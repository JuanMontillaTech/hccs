

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static ERP.Domain.Constants.ConstantsType;

namespace ERP.Services.Implementations
{
    public class AccountingProcess : IAccountingProcess
    {
        private readonly IGenericRepository<Journal> _repJournals;
        private readonly IGenericRepository<JournaDetails> _repJournaDetails;
        private readonly IGenericRepository<Box> _repoBox;
        private readonly IGenericRepository<Concept> _repoConcept;
        private readonly IGenericRepository<ConfigurationSell> _repConfigurationSell;
        private readonly IGenericRepository<ConfigurationPurchase> _repoConfigurationPurchase;

        public AccountingProcess(
            IGenericRepository<Journal> repJournals,
            IGenericRepository<Box> repoBox,
            IGenericRepository<Concept> repoConcept,
            IGenericRepository<ConfigurationSell> repConfigurationSell,
            IGenericRepository<ConfigurationPurchase> repoConfigurationPurchase,
            IGenericRepository<JournaDetails> repJournaDetails

            )
        {
            _repJournals = repJournals;
            _repoBox = repoBox;
            _repoConcept = repoConcept;
            _repConfigurationSell = repConfigurationSell;
            _repoConfigurationPurchase = repoConfigurationPurchase;
            _repJournaDetails = repJournaDetails;
        }

        public async Task PostJournalEntry(Transactions transactions)
        {

            await AccountingTransaction(transactions);


        }

        public async Task UpdateJournalEntry(Transactions transactions)
        {
            await AccountingTransaction(transactions);
        }
        public async Task DeleteJournalEntry(Transactions transactions)
        {
            if (transactions.Id != Guid.Empty)
            {
                var transanfound = await _repJournals.Find(x => x.TypeRegisterId == transactions.Id).FirstOrDefaultAsync();
                if (transanfound != null)
                {
                    transanfound.IsActive = false;
                    await _repJournals.Update(transanfound);
                    await _repJournals.SaveChangesAsync();
                }
            }
        }

        private async Task<bool> AccountingTransaction(
              Transactions document)
        {
            bool result = true;

            var configurationSell = await _repConfigurationSell.FirstOrDefaultAsync();

            var configurationPurchase = await _repoConfigurationPurchase.FirstOrDefaultAsync();

            Journal journal = new Journal();
            var journalFound = await _repJournals.Find(x => x.TypeRegisterId == document.Id).Include(x => x.JournaDetails).FirstOrDefaultAsync();
            if (journalFound != null)
            {
                journal = journalFound;
            }

            journal.Code = document.Code;
            journal.Date = DateTime.Now;
            journal.Reference = document.Reference;

            journal.TypeRegisterId = document.Id;

            List<JournaDetails> journaDetailsList = new List<JournaDetails>();

            if (journalFound.JournaDetails.Count > 0)
            {
                foreach (var item in journalFound.JournaDetails)
                {


                    item.IsActive = false;

                    await _repJournaDetails.Update(item);

                    await _repJournaDetails.SaveChangesAsync();
                }
            }


            switch (document.TransactionsType)
            {

                case (int)TypeAccountingTransaction.InvoiceCash:

                    if (document.TotalTax > 0)
                    {
                        /*  TotalTax: Campo con la suma de todos los impuestos    
                            TotalAmountTax: Campo del precio sin impuesto
                            GlobalTotalTax  Total mas impuesto */

                        // cuenta de caja
                        var box = await _repoBox.GetById(document.BoxId.Value);
                        JournaDetails journaDebit = NewJournaDetailsRow(
                            journal.Id, box.LedgerAccountId.Value,
                            document.GlobalTotalTax, 0);

                        journaDetailsList.Add(journaDebit);

                        //Cuenta de venta del articulo
                        foreach (var documentTransactionsDetail in document.TransactionsDetails)
                        {

                            var concept = await _repoConcept.GetById(documentTransactionsDetail.ReferenceId.Value);
                            if (concept != null)
                            {
                                if (concept.AccountSalesId.HasValue)
                                {
                                    document.TotalAmountTax = document.TotalAmountTax - documentTransactionsDetail.TotalTax;
                                    JournaDetails NewjournaConcept = NewJournaDetailsRow(
                                     journal.Id, concept.AccountSalesId.Value, 0, documentTransactionsDetail.TotalTax);
                                    journaDetailsList.Add(NewjournaConcept);
                                }

                            }

                        }
                        //Cuenta de venta
                        if (document.TotalAmountTax > 0)
                        {
                            JournaDetails journaCreditAmmount = NewJournaDetailsRow(
                    journal.Id, configurationSell.AccountSelling.Value, 0, document.TotalAmountTax);
                            journaDetailsList.Add(journaCreditAmmount);
                        }


                        //cuenta de impuesto

                        JournaDetails journaTax = NewJournaDetailsRow(
                            journal.Id, configurationSell.AccountITBISexpenses.Value, 0, document.TotalTax);
                        journaDetailsList.Add(journaTax);




                    }




                    break;

                case (int)TypeAccountingTransaction.ExpenseCash:
                    foreach (var documentTransactionsDetail in document.TransactionsDetails)
                    {
                        Guid boxAccount = Guid.Empty;
                        if (document.BoxId != null)
                        {
                            var box = await _repoBox.GetById(document.BoxId.Value);
                            if (box.LedgerAccountId != null) boxAccount = box.LedgerAccountId.Value;
                        }

                        Guid accountSellId;

                        decimal ammount = documentTransactionsDetail.TotalTax <= 0
                            ? documentTransactionsDetail.Total
                            : documentTransactionsDetail.TotalTax;
                        //Add Debit
                        if (boxAccount == Guid.Empty) continue;
                        JournaDetails journaDebit = NewJournaDetailsRow(
                            journal.Id, boxAccount,
                            0, ammount);

                        journaDetailsList.Add(journaDebit);

                        var concept = await _repoConcept.GetById(documentTransactionsDetail.ReferenceId.Value);

                        accountSellId = concept.AccountPurchaseId.HasValue
                            ? concept.AccountPurchaseId.Value
                            : configurationPurchase.AccountPurchase.Value;


                        JournaDetails journaCreditAmmount = NewJournaDetailsRow(
                            journal.Id, accountSellId, ammount, 0);
                        journaDetailsList.Add(journaCreditAmmount);

                        if (documentTransactionsDetail.Tax > 0)
                        {
                            JournaDetails journaTax = NewJournaDetailsRow(
                                journal.Id, accountSellId, 0, documentTransactionsDetail.Tax);
                            journaDetailsList.Add(journaTax);
                        }

                    }
                    break;

                default:

                    break;
            }
            if (journaDetailsList.Count > 0)
            {
                journal.JournaDetails = journaDetailsList;

                await _repJournals.InsertAsync(journal);
                await _repJournals.SaveChangesAsync();
            }

            return result;
        }
        private JournaDetails NewJournaDetailsRow(Guid JournalId,
           Guid LedgerAccountId,
           decimal Debit, decimal Credit)
        {
            JournaDetails journaDetails = new JournaDetails();

            journaDetails.JournalId = JournalId;

            journaDetails.LedgerAccountId = LedgerAccountId;

            journaDetails.Debit = Debit;

            journaDetails.Credit = Credit;


            return journaDetails;
        }



    }
}

