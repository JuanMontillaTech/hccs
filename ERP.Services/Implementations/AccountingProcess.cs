

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger<AccountingProcess> _logger;
        public AccountingProcess(
            IGenericRepository<Journal> repJournals,
            IGenericRepository<Box> repoBox,
            IGenericRepository<Concept> repoConcept,
            IGenericRepository<ConfigurationSell> repConfigurationSell,
            IGenericRepository<ConfigurationPurchase> repoConfigurationPurchase,
            IGenericRepository<JournaDetails> repJournaDetails,
            ILogger<AccountingProcess> logger

            )
        {
            _repJournals = repJournals;
            _repoBox = repoBox;
            _repoConcept = repoConcept;
            _repConfigurationSell = repConfigurationSell;
            _repoConfigurationPurchase = repoConfigurationPurchase;
            _repJournaDetails = repJournaDetails;
            _logger = logger;
        }


        public async Task PostJournaSellTransactionReceipt(TransactionReceipt _TransactionReceipt)
        {
            Guid boxAccount = await GetIdBox(_TransactionReceipt.BoxId);

            switch (_TransactionReceipt.Type)
            {
                case (int)Document.IncomeReceipt:
                    //Asiento de banco 
                    Journal journal = await SaveIncomeReceiptorSell(_TransactionReceipt, boxAccount);

                    break;
                case (int)Document.ExpenseReceipt:
                    //Asiento de banco 
                    Journal journal2 = await SaveExpenseReceipttorSell(_TransactionReceipt, boxAccount);

                    break;
            }






        }
        private async Task<Journal> SaveExpenseReceipttorSell(TransactionReceipt _TransactionReceipt, Guid boxAccount)
        {
            //Asiento de banco 
            Journal journal = new Journal();
            var journalFound = await _repJournals.Find(x => x.TypeRegisterId == _TransactionReceipt.Id).FirstOrDefaultAsync();
            if (journalFound != null)
            {
                journal = journalFound;
            }

            journal.Code = _TransactionReceipt.Document;
            journal.Date = DateTime.Now;
            journal.Reference = _TransactionReceipt.Reference;

            journal.TypeRegisterId = _TransactionReceipt.Id;
            await CleanForInsertOUpdate(_TransactionReceipt.Id);

            List<JournaDetails> journaDetailsList = new List<JournaDetails>();

          

            foreach (var item in _TransactionReceipt.TransactionReceiptDetails)
            {

                JournaDetails journaDetails = NewJournaDetailsRow(journal.Id, item.referenceId,   item.Paid,0);
                journaDetailsList.Add(journaDetails);

            }
              //Asiento de banco 
            JournaDetails journalBank = NewJournaDetailsRow(journal.Id, boxAccount,0, _TransactionReceipt.Total);
            journaDetailsList.Add(journalBank);

            journal.JournaDetails = journaDetailsList;
            if (journal.Id != Guid.Empty)
            {
                try
                {
                    await _repJournals.Update(journal);
                    await _repJournals.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("U Error al guardar la journal {0}", ex.Message);
                }
            }
            else
            {
                try
                {
                    await _repJournals.InsertAsync(journal);
                    await _repJournals.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("I Error al guardar la journal {0}", ex.Message);
                }
            }

            return journal;
        }

        private async Task<Journal> SaveIncomeReceiptorSell(TransactionReceipt _TransactionReceipt, Guid boxAccount)
        {
            //Asiento de banco 
            Journal journal = new Journal();
            var journalFound = await _repJournals.Find(x => x.TypeRegisterId == _TransactionReceipt.Id).FirstOrDefaultAsync();
            if (journalFound != null)
            {
                journal = journalFound;
            }

            journal.Code = _TransactionReceipt.Document;
            journal.Date = DateTime.Now;
            journal.Reference = _TransactionReceipt.Reference;

            journal.TypeRegisterId = _TransactionReceipt.Id;
            await CleanForInsertOUpdate(_TransactionReceipt.Id);

            List<JournaDetails> journaDetailsList = new List<JournaDetails>();

            //Asiento de banco 
            JournaDetails journalBank = NewJournaDetailsRow(journal.Id, boxAccount, _TransactionReceipt.Total, 0);
            journaDetailsList.Add(journalBank);

            foreach (var item in _TransactionReceipt.TransactionReceiptDetails)
            {

                JournaDetails journaDetails = NewJournaDetailsRow(journal.Id, item.referenceId, 0, item.Paid);
                journaDetailsList.Add(journaDetails);

            }

            journal.JournaDetails = journaDetailsList;
            if (journal.Id != Guid.Empty)
            {
                try
                {
                    await _repJournals.Update(journal);
                    await _repJournals.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("U Error al guardar la journal {0}", ex.Message);
                }
            }
            else
            {
                try
                {
                    await _repJournals.InsertAsync(journal);
                    await _repJournals.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("I Error al guardar la journal {0}", ex.Message);
                }
            }

            return journal;
        }


        #region Transactionss
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
                var transanfound = await _repJournals.Find(x => x.TypeRegisterId == transactions.Id).FirstOrDefaultAsync(x => x.IsActive == true);
                if (transanfound != null)
                {
                    transanfound.IsActive = false;
                    try
                    {
                        await _repJournals.Update(transanfound);
                        await _repJournals.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("DeleteJournalEntry : Error al actualizar no activo el registro{0} en accounting process", ex.Message);
                    }
                }
            }
        }
        private async Task<bool> AccountingTransaction(
              Transactions document)
        {
            bool result = true;

            var configurationSell = await _repConfigurationSell.FirstOrDefaultAsync();



            Journal journal = new Journal();


            journal.Code = document.Code;
            journal.Date = DateTime.Now;
            journal.Reference = document.Reference;

            journal.TypeRegisterId = document.Id;

            List<JournaDetails> journaDetailsList = new List<JournaDetails>();
 
            await CleanForInsertOUpdate(document.Id);

            switch (document.TransactionsType)
            {

                case (int)Document.InvoiceCash:

                    await HandleInvoiceCashTransaction(document, configurationSell, journal, journaDetailsList);

                    break;

                case (int)Document.ExpenseCash:

                    await HandleExpenseCashTransaction(document, journal, journaDetailsList);

                    break;

                default:

                    break;
            }
            if (journaDetailsList.Count > 0)
            {
                journal.JournaDetails = journaDetailsList;

                try
                {

                    await _repJournals.InsertAsync(journal);
                    await _repJournals.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error al guardar la journal {0}", ex.Message);
                }




            }

            return result;
        }

        private async Task CleanForInsertOUpdate(Guid IdFound)
        {
            var journalFound = await _repJournals.Find(x => x.TypeRegisterId == IdFound).Include(x => x.JournaDetails).FirstOrDefaultAsync();

            if (journalFound != null)
            {
                foreach (var item in journalFound.JournaDetails)
                {


                    item.IsActive = false;

                    try
                    {

                        await _repJournaDetails.Update(item);
                        await _repJournaDetails.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error al actualizar el registro{0} en accounting process", ex.Message);
                    }
                }
            }

        }

        public async Task<Guid> GetIdBox(Guid id)
        {
            var box = await _repoBox.GetById(id);
            if (box != null)
            {
                if (box.LedgerAccountId.HasValue)
                    return box.LedgerAccountId.Value;
            }
            return Guid.Empty;

        }
        private async Task HandleExpenseCashTransaction(Transactions document, Journal journal, List<JournaDetails> journalDetailsList)
        {
            // Validación de entrada
            if (document == null || journal == null || journalDetailsList == null)
            {
                _logger.LogError("Los argumentos no pueden ser nulos.");
                return;
            }

            if (document.BoxId == null)
            {
                _logger.LogError("El campo BoxId es requerido para transacciones de tipo ExpenseCash.");
                return;
            }

            // Obtener la cuenta de caja
            /*  var box = await _repoBox.GetById(document.BoxId.Value);
              if (box == null || box.LedgerAccountId == null)
              {
                  _logger.LogError("No se pudo encontrar la cuenta de caja asociada.");
              }
              Guid boxAccount = box.LedgerAccountId.Value;*/
            Guid boxAccount = await GetIdBox(document.BoxId.Value);
            // Obtener la configuración de compra
            var configurationPurchase = await _repoConfigurationPurchase.FirstOrDefaultAsync();
            decimal amount = document.TotalTax <= 0
                ? document.GlobalTotal
                : document.GlobalTotalTax;

            // Agregar débito a la cuenta de caja
            JournaDetails journalDebit = NewJournaDetailsRow(journal.Id, boxAccount, 0, amount);
            journalDetailsList.Add(journalDebit);

            // Procesar los detalles de la transacción
            foreach (var documentTransactionsDetail in document.TransactionsDetails)
            {
                // Validar ReferenceId
                if (documentTransactionsDetail.ReferenceId == null)
                {
                    _logger.LogError("El campo ReferenceId es requerido para los detalles de la transacción.");
                    continue;
                }



                // Obtener la cuenta de compra del concepto
                var concept = await _repoConcept.GetById(documentTransactionsDetail.ReferenceId.Value);
                if (concept != null)
                {
                    if (concept.AccountPurchaseId.HasValue)
                    {
                        decimal restTotal = document.TotalTax <= 0
                                               ? documentTransactionsDetail.Price
                                               : documentTransactionsDetail.PriceWithTax;
                        amount = amount - restTotal;
                        JournaDetails journalCreditAmount1 = NewJournaDetailsRow(journal.Id, concept.AccountPurchaseId.Value, restTotal, 0);
                        journalDetailsList.Add(journalCreditAmount1);
                    }
                }
            }






            // Agregar crédito por impuesto (si aplica)
            if (document.TotalTax > 0)
            {
                if (!configurationPurchase.AccountTaxholding.HasValue)
                {
                    _logger.LogError("No se pudo encontrar la cuenta de impuesto asociada.");
                }
                amount -= document.TotalTax;
                JournaDetails journalTax = NewJournaDetailsRow(journal.Id, configurationPurchase.AccountTaxholding.Value, document.TotalTax, 0);
                journalDetailsList.Add(journalTax);
            }
            // Agregar crédito a la cuenta de compra
            if (amount > 0)
            {
                JournaDetails journalCreditAmount = NewJournaDetailsRow(journal.Id, configurationPurchase.AccountPurchase.Value, amount, 0);
                journalDetailsList.Add(journalCreditAmount);
            }

        }
        private async Task HandleInvoiceCashTransaction(Transactions document, ConfigurationSell configurationSell, Journal journal, List<JournaDetails> journaDetailsList)
        {
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
            else
            {
                /*  
                 --TotalAmount: Campo del precio sin impuesto
                  --GlobalTotal Total mas impuesto 
                  */

                // cuenta de caja
                var box = await _repoBox.GetById(document.BoxId.Value);
                JournaDetails journaDebit = NewJournaDetailsRow(
                    journal.Id, box.LedgerAccountId.Value,
                    document.GlobalTotal, 0);

                journaDetailsList.Add(journaDebit);

                //Cuenta de venta del articulo
                foreach (var documentTransactionsDetail in document.TransactionsDetails)
                {

                    var concept = await _repoConcept.GetById(documentTransactionsDetail.ReferenceId.Value);
                    if (concept != null)
                    {
                        if (concept.AccountSalesId.HasValue)
                        {
                            document.TotalAmount = document.TotalAmount - documentTransactionsDetail.Total;
                            JournaDetails NewjournaConcept = NewJournaDetailsRow(
                             journal.Id, concept.AccountSalesId.Value, 0, documentTransactionsDetail.Total);
                            journaDetailsList.Add(NewjournaConcept);
                        }

                    }

                }
                //Cuenta de venta
                if (document.TotalAmount > 0)
                {
                    JournaDetails journaCreditAmmount = NewJournaDetailsRow(
            journal.Id, configurationSell.AccountSelling.Value, 0, document.TotalAmount);
                    journaDetailsList.Add(journaCreditAmmount);
                }




            }
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
    #endregion
}

