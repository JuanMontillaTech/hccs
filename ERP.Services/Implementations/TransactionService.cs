using ERP.Domain.Command;
using ERP.Domain.Dtos;
using ERP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Services.Constants;
using Org.BouncyCastle.Crypto;
using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Amazon.S3.Model.Internal.MarshallTransformations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Globalization;

namespace ERP.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        public readonly IGenericRepository<Transactions> _RepTrasacion;
        public readonly IGenericRepository<Form> _RepForm;
        public readonly IGenericRepository<TransactionsDetails> _RepTrasacionDetails;
        public readonly IGenericRepository<ConfigurationSell> _RepConfigurationSell;
        public readonly IGenericRepository<Concept> _RepoConcept;
        public readonly IGenericRepository<ConfigurationPurchase> _RepoConfigurationPurchase;
        public readonly IGenericRepository<Journal> _RepJournals;
        public readonly IGenericRepository<JournaDetails> _RepJournalsDetails;
        public readonly IGenericRepository<PaymentMethod> _RepPaymentMethod;

        public TransactionService(IGenericRepository<Transactions> repTrasacion, IGenericRepository<Form> repForm, IGenericRepository<TransactionsDetails> repTrasacionDetails,
            IGenericRepository<ConfigurationSell> repConfigurationSell, IGenericRepository<ConfigurationPurchase> repoConfigurationPurchase,
            IGenericRepository<Journal> repJournals,
            IGenericRepository<JournaDetails> repJournalsDetails,
            IGenericRepository<Concept> RepoConcept,
            IGenericRepository<PaymentMethod> repPaymentMethod)
        {
            _RepTrasacion = repTrasacion;
            _RepForm = repForm;
            _RepTrasacionDetails = repTrasacionDetails;
            _RepConfigurationSell = repConfigurationSell;
            _RepoConfigurationPurchase = repoConfigurationPurchase;
            _RepJournals = repJournals;
            _RepoConcept = RepoConcept;
            _RepJournalsDetails = repJournalsDetails;
            _RepPaymentMethod = repPaymentMethod;
        }

        public async Task<Transactions> TransactionProcess(Transactions transactions, Guid formId)
        {

            try
            {


                //Valide data is correct
                //programing procces to Create
                if (transactions.Id == Guid.Empty)
                {

                    var rowForm = await _RepForm.GetById(formId);
                    if (rowForm.AllowSequence != null)
                    {

                    if (rowForm.AllowSequence.Value)
                    {

                        rowForm.Sequence = rowForm.Sequence.Value + 1;
                        transactions.Code = rowForm.Prefix + rowForm.Sequence.ToString();

                    }
                    }

                    await _RepTrasacion.Insert(transactions);

                    switch (transactions.TransactionsType)
                    {
                        case (int)Constants.Constants.Document.InvoiceCredit:

                            await AccountingTransaction(TypeAccountingTransaction.SellLayaway, transactions);

                            break;
                        case (int)Constants.Constants.Document.InvoiceCash:

                            await AccountingTransaction(TypeAccountingTransaction.Sell, transactions);

                            break;
                        case (int)Constants.Constants.Document.InvoceReturn: 

                            break;
                    
                        case (int)Constants.Constants.Document.ExpenseCash:

                            await AccountingTransaction(TypeAccountingTransaction.Purchase, transactions);

                            break;
                        case (int)Constants.Constants.Document.ExpenseCredit:

                            await AccountingTransaction(TypeAccountingTransaction.PurchaseLayaway, transactions);

                            break;
                    }
                  await   _RepTrasacion.SaveChangesAsync();
                }
                else
                {
                    await _RepTrasacion.Update(transactions);


                    var _TransantionDetealleForDelete = await _RepTrasacionDetails.Find(x => x.TransactionsId == transactions.Id).ToListAsync();


                    foreach (var item in _TransantionDetealleForDelete)
                    {
                        var resulDelte = await _RepTrasacionDetails.Delete(item.Id);
                        await _RepTrasacionDetails.SaveChangesAsync();
                    }
                    var TransactionsDetailsList = transactions.TransactionsDetails;
                    transactions.TransactionsDetails = TransactionsDetailsList;
                    await _RepTrasacionDetails.InsertArray(transactions.TransactionsDetails);

                    await _RepTrasacion.SaveChangesAsync();



                }
                //Insert DB
                //Create accounting detalles
                return transactions;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }
        }
    

        public enum TypeAccountingTransaction
        {
            SellLayaway = 0,
            Sell = 1,
            PurchaseLayaway = 10,
            Purchase = 11,

        }
        #region AccountingTransaccion
        public async Task<bool> AccountingTransaction(TypeAccountingTransaction typeAccountingTransaction, Transactions document)
        {
            bool Result = true;
            var ConfigurationSell = await _RepConfigurationSell.FirstOrDefaultAsync();

            var ConfigurationPurchase = await _RepoConfigurationPurchase.FirstOrDefaultAsync();
            Journal journal = new Journal();


            switch (typeAccountingTransaction)
            {
                case TypeAccountingTransaction.SellLayaway:

                    journal.Code = document.Code;

                    journal.Date = DateTime.Now;

                    journal.Reference = document.Reference;

                    var TransactionGobalTotal = await GetdocumentGlobalTotal(document.TransactionsDetails);

                    List<JournaDetails> journaDetailsList = new List<JournaDetails>();

                    //Cuentas por cobrar

                    if (TransactionGobalTotal.Total > 0)
                    {
                        JournaDetails journaDetails = NewJournaDetailsRow(
                        journal.Id,
                        ConfigurationSell.Accountcollect.Value, TransactionGobalTotal.Total, 0);
                        journaDetailsList.Add(journaDetails);
                    }

                    //Cobrar ITBIS

                    if (TransactionGobalTotal.Tax > 0)
                    {
                        JournaDetails journaDetails = NewJournaDetailsRow(
                        journal.Id,
                        ConfigurationSell.AccountITBISexpenses.Value, 0, TransactionGobalTotal.Tax);
                        journaDetailsList.Add(journaDetails);
                    }

                    //Cuentas de venta

                    if (TransactionGobalTotal.Total > 0)
                    {

                        JournaDetails journaDetails = NewJournaDetailsRow(
                        journal.Id,
                        ConfigurationSell.AccountSelling.Value, 0, TransactionGobalTotal.Total);
                        journaDetailsList.Add(journaDetails);

                        ///Registro al concepto
                        ///Buscar lo que se cobro a a ese articulo
                        ///
                        foreach (var TransactionsD in document.TransactionsDetails)
                        {
                            var RepoConcept = await _RepoConcept.GetById(TransactionsD.ReferenceId);
                            if (RepoConcept != null)
                            {
                                if (RepoConcept.AccountSalesId.HasValue)
                                {
                                    JournaDetails journaDetailsConcept = NewJournaDetailsRow(
                                    journal.Id, RepoConcept.AccountSalesId.Value, 0, TransactionsD.Total);
                                    journaDetailsList.Add(journaDetailsConcept);
                                }
                                //Resta el inventario
                                 decimal StockExist =RepoConcept.Stock.HasValue ? RepoConcept.Stock.Value :0;
                                if (StockExist >= 1)
                                {
                                    StockExist = StockExist - TransactionsD.Amount;
                                }

                            }

                        }


                    }

                    if (journaDetailsList.Count > 0)
                    {

                        journal.JournaDetails = journaDetailsList;

                        await _RepJournals.Insert(journal);

                    }

                    break;

                case TypeAccountingTransaction.Sell:

                    journal.Code = document.Code;

                    journal.Date = DateTime.Now;

                    journal.Reference = document.Reference;

                    var payment = _RepPaymentMethod.Find(x => x.Id == document.PaymentMethodId).Include(x => x.Banks).FirstOrDefault();

                    var TransactionGobalTotal2 = await GetdocumentGlobalTotal(document.TransactionsDetails);

                    List<JournaDetails> journaDetailsList2 = new List<JournaDetails>();

                    //Cuentas Banks

                    if (TransactionGobalTotal2.Total > 0)
                    {
                        if (payment.Banks != null)
                        {


                            if (payment.Banks.LedgerAccountId.HasValue)
                            {
                                JournaDetails journaDetails = NewJournaDetailsRow(
                                journal.Id, payment.Banks.LedgerAccountId.Value, TransactionGobalTotal2.Total, 0);
                                journaDetailsList2.Add(journaDetails);
                            }
                        }
                    }

                    //Cobrar ITBIS

                    if (TransactionGobalTotal2.Tax > 0)
                    {
                        JournaDetails journaDetails = NewJournaDetailsRow(
                        journal.Id,
                        ConfigurationSell.AccountITBISexpenses.Value, 0, TransactionGobalTotal2.Tax);
                        journaDetailsList2.Add(journaDetails);
                    }

                    //Cuentas de venta

                    if (TransactionGobalTotal2.Total > 0)
                    {
                        foreach (var TransactionsD in document.TransactionsDetails)
                        {
                            var RepoConcept = await _RepoConcept.GetById(TransactionsD.ReferenceId);
                            if (RepoConcept != null)
                            {
                                if (RepoConcept.AccountSalesId.HasValue)
                                {
                                    JournaDetails journaDetailsConcept = NewJournaDetailsRow(
                                    journal.Id, RepoConcept.AccountSalesId.Value, 0, TransactionsD.Total);
                                    journaDetailsList2.Add(journaDetailsConcept);
                                }
                                //Resta el inventario
                                 decimal StockExist =RepoConcept.Stock.HasValue ? RepoConcept.Stock.Value :0;
                                if (StockExist >= 1)
                                {
                                    StockExist = StockExist - TransactionsD.Amount;
                                }
                            }

                        }

                        JournaDetails journaDetails = NewJournaDetailsRow(
                        journal.Id,
                        ConfigurationSell.AccountSelling.Value, 0, TransactionGobalTotal2.Total);
                        journaDetailsList2.Add(journaDetails);
                    }

                    if (journaDetailsList2.Count > 0)
                    {

                        journal.JournaDetails = journaDetailsList2;

                        await _RepJournals.Insert(journal);

                    }

                    break;

                case TypeAccountingTransaction.PurchaseLayaway:

                    journal.Code = document.Code;

                    journal.Date = DateTime.Now;

                    journal.Reference = document.Reference;

                    journal.TypeRegisterId = document.Id;

                    var TransactionPurchaseLayawayGobalTotal = await GetdocumentGlobalTotal(document.TransactionsDetails);

                    List<JournaDetails> PurchaseLayawatDetailsList = new List<JournaDetails>();

                    //Compra 

                    if (TransactionPurchaseLayawayGobalTotal.Total > 0)
                    {
                        JournaDetails journaDetails = NewJournaDetailsRow(
                        journal.Id,
                        ConfigurationPurchase.AccountPurchase.Value, TransactionPurchaseLayawayGobalTotal.Total, 0);

                        PurchaseLayawatDetailsList.Add(journaDetails);
                        foreach (var TransactionsD in document.TransactionsDetails)
                        {
                            var RepoConcept = await _RepoConcept.GetById(TransactionsD.ReferenceId);
                            if (RepoConcept != null)
                            {
                                if (RepoConcept.AccountCostId.HasValue)
                                {
                                    JournaDetails journaDetailsConcept = NewJournaDetailsRow(
                                    journal.Id, RepoConcept.AccountCostId.Value, TransactionsD.Total, 0);
                                    PurchaseLayawatDetailsList.Add(journaDetailsConcept);
                                }
                                //Resta el inventario
                                 decimal StockExist =RepoConcept.Stock.HasValue ? RepoConcept.Stock.Value :0;
                                if (StockExist >= 1)
                                {
                                    StockExist = StockExist + TransactionsD.Amount;
                                }
                            }
                        }

                    }

                    //Cobrar ITBIS

                    if (TransactionPurchaseLayawayGobalTotal.Tax > 0)
                    {
                        JournaDetails journaDetails = NewJournaDetailsRow(
                        journal.Id,
                        ConfigurationPurchase.AccountTaxholding.Value, TransactionPurchaseLayawayGobalTotal.Tax, 0);

                        PurchaseLayawatDetailsList.Add(journaDetails);
                    }


                    if (TransactionPurchaseLayawayGobalTotal.Total > 0)
                    {

                        JournaDetails journaDetails = NewJournaDetailsRow(
                        journal.Id, ConfigurationPurchase.AccountPay.Value, 0, TransactionPurchaseLayawayGobalTotal.Total);

                        PurchaseLayawatDetailsList.Add(journaDetails);

                    }

                    if (PurchaseLayawatDetailsList.Count > 0)
                    {

                        journal.JournaDetails = PurchaseLayawatDetailsList;

                        await _RepJournals.Insert(journal);

                    }

                    break;

                case TypeAccountingTransaction.Purchase:
                    var paymentnPurcha = await _RepPaymentMethod.GetById(document.PaymentMethodId);

                    journal.Code = document.Code;

                    journal.Date = DateTime.Now;

                    journal.Reference = document.Reference;




                    var TransactionPurchaselTotal = await GetdocumentGlobalTotal(document.TransactionsDetails);

                    List<JournaDetails> PurchaseDetailsList = new List<JournaDetails>();

                    //Compra 

                    if (TransactionPurchaselTotal.Total > 0)
                    {
                        JournaDetails journaDetails = NewJournaDetailsRow(
                        journal.Id,
                        ConfigurationPurchase.AccountPurchase.Value, TransactionPurchaselTotal.Total, 0);
                        PurchaseDetailsList.Add(journaDetails);

                        ///Registro al concepto
                        ///Buscar lo que se cobro a a ese articulo 
                        foreach (var TransactionsD in document.TransactionsDetails)
                        {
                            var RepoConcept = await _RepoConcept.GetById(TransactionsD.ReferenceId);
                            if (RepoConcept != null)
                            {
                                if (RepoConcept.AccountCostId.HasValue)
                                {
                                    JournaDetails journaDetailsConcept = NewJournaDetailsRow(
                                    journal.Id, RepoConcept.AccountCostId.Value, TransactionsD.Total, 0);
                                    PurchaseDetailsList.Add(journaDetailsConcept);
                                }
                                //Resta el inventario
                                 decimal StockExist =RepoConcept.Stock.HasValue ? RepoConcept.Stock.Value :0;
                                if (StockExist >= 1)
                                {
                                    StockExist = StockExist + TransactionsD.Amount;
                                }

                            }
                        }

                    }

                    //ITBIS

                    if (TransactionPurchaselTotal.Tax > 0)
                    {
                        JournaDetails journaDetails = NewJournaDetailsRow(
                        journal.Id,
                        ConfigurationPurchase.AccountTaxholding.Value, TransactionPurchaselTotal.Tax, 0);

                        PurchaseDetailsList.Add(journaDetails);
                    }


                    if (TransactionPurchaselTotal.Total > 0)
                    {
                        if (paymentnPurcha.Banks != null)
                        {
                            if (paymentnPurcha.Banks.LedgerAccountId.HasValue)
                            {
                                JournaDetails journaDetails = NewJournaDetailsRow(
                                journal.Id, paymentnPurcha.Banks.LedgerAccountId.Value, 0, TransactionPurchaselTotal.Total);

                                PurchaseDetailsList.Add(journaDetails);
                            }
                        }

                    }



                    if (PurchaseDetailsList.Count > 0)
                    {

                        journal.JournaDetails = PurchaseDetailsList;

                        await _RepJournals.Insert(journal);

                    }

                    break;

                default:

                    break;
            }


            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="JournalId"></param>
        /// <param name="LedgerAccountId"></param>
        /// <param name="Credit"></param>
        /// <param name="Debit"></param>
        /// <returns></returns>
        public JournaDetails NewJournaDetailsRow(Guid JournalId,
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


        public async Task<TransactionsDetails> GetdocumentGlobalTotal(List<TransactionsDetails> document)
        {

            TransactionsDetails transactions = new TransactionsDetails();

            transactions.Total = document.Sum(x => x.Total);
            transactions.Tax = document.Sum(x => x.Tax);

            return transactions;





        }   
      
        #endregion
    }
}
