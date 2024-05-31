

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.Implementations
{
    public class AccountingProcess : IAccountingProcess
    {
        private readonly IGenericRepository<Journal> _repJournals;
        private readonly IGenericRepository<Box> _repoBox;
        private readonly IGenericRepository<Concept> _repoConcept;
        private readonly IGenericRepository<ConfigurationSell> _repConfigurationSell;
        private readonly IGenericRepository<ConfigurationPurchase> _repoConfigurationPurchase;

        public AccountingProcess(
            IGenericRepository<Journal> repJournals,
            IGenericRepository<Box> repoBox,
            IGenericRepository<Concept> repoConcept,
            IGenericRepository<ConfigurationSell> repConfigurationSell,
            IGenericRepository<ConfigurationPurchase> repoConfigurationPurchase)
        {
            _repJournals = repJournals;
            _repoBox = repoBox;
            _repoConcept = repoConcept;
            _repConfigurationSell = repConfigurationSell;
            _repoConfigurationPurchase = repoConfigurationPurchase;
        }

        public async Task PostJournalEntry(Transactions transactions)
        {

            await AccountingTransaction(transactions);


        }

        public Task UpdateJournalEntry(Transactions transactions)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> AccountingTransaction(
              Transactions document)
        {
            bool result = true;

            var configurationSell = await _repConfigurationSell.FirstOrDefaultAsync();

            var configurationPurchase = await _repoConfigurationPurchase.FirstOrDefaultAsync();

            if (document.Id != Guid.Empty)
            {
                var transanfound = await _repJournals.Find(x => x.TypeRegisterId == document.Id).FirstOrDefaultAsync();
                if (transanfound != null) transanfound.IsActive = false;
            }


            Journal journal = new Journal();

            journal.Code = document.Code;

            journal.Date = DateTime.Now;

            journal.Reference = document.Reference;

            journal.TypeRegisterId = document.Id;
            List<JournaDetails> journaDetailsList = new List<JournaDetails>();
            switch (document.TransactionsType)
            {

                case (int)TypeAccountingTransaction.Sell:

                    foreach (var documentTransactionsDetail in document.TransactionsDetails)
                    {

                        //si la transaccion es de impuesto se toma el total de impuesto
                        //si no se toma el total de la transaccion

                        if (documentTransactionsDetail.TotalTax > 0)
                        {
                            // cuenta de caja
                            var box = await _repoBox.GetById(document.BoxId.Value);
                            JournaDetails journaDebit = NewJournaDetailsRow(
                                journal.Id, box.LedgerAccountId.Value,
                                documentTransactionsDetail.TotalTax, 0);

                            journaDetailsList.Add(journaDebit);
                            //cuenta de impuesto

                            JournaDetails journaTax = NewJournaDetailsRow(
                                journal.Id, configurationSell.AccountITBISexpenses.Value, 0, documentTransactionsDetail.Tax);
                            journaDetailsList.Add(journaTax);


                            //Cuenta de venta
                            Guid accountSellId;
                            var concept = await _repoConcept.GetById(documentTransactionsDetail.ReferenceId.Value);

                            accountSellId = concept.AccountSalesId.HasValue
                                ? concept.AccountSalesId.Value
                                : configurationSell.AccountSelling.Value;
                            JournaDetails journaCreditAmmount = NewJournaDetailsRow(
                    journal.Id, accountSellId, 0, documentTransactionsDetail.TotalTax);
                            journaDetailsList.Add(journaCreditAmmount);

                        }
                        else
                        {



                            Guid boxAccount = Guid.Empty;
                            if (document.BoxId != null)
                            {
                                var box = await _repoBox.GetById(document.BoxId.Value);
                                if (box.LedgerAccountId != null) boxAccount = box.LedgerAccountId.Value;
                            }




                            decimal ammount = documentTransactionsDetail.TotalTax <= 0
                                ? documentTransactionsDetail.Total
                                : documentTransactionsDetail.TotalTax;


                            if (boxAccount == Guid.Empty) continue;
                            JournaDetails journaDebit = NewJournaDetailsRow(
                                journal.Id, boxAccount,
                                ammount, 0);

                            journaDetailsList.Add(journaDebit);

                            var concept = await _repoConcept.GetById(documentTransactionsDetail.ReferenceId.Value);
                            Guid accountSellId;
                            accountSellId = concept.AccountSalesId.HasValue
                                ? concept.AccountSalesId.Value
                                : configurationSell.AccountSelling.Value;


                            JournaDetails journaCreditAmmount = NewJournaDetailsRow(
                                journal.Id, accountSellId, 0, ammount);
                            journaDetailsList.Add(journaCreditAmmount);

                            if (documentTransactionsDetail.Tax > 0)
                            {
                                JournaDetails journaTax = NewJournaDetailsRow(
                                    journal.Id, accountSellId, 0, documentTransactionsDetail.Tax);
                                journaDetailsList.Add(journaTax);
                            }
                        }

                    }




                    break;

                case (int)TypeAccountingTransaction.Purchase:
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


        private enum TypeAccountingTransaction
        {
            SellLayaway = 0,
            Sell = 1,
            PurchaseLayaway = 10,
            Purchase = 11,
        }
    }
}

