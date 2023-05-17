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
        private readonly IGenericRepository<Transactions> _repTrasacion;
        private readonly IGenericRepository<Form> _repForm;
        private readonly IGenericRepository<TransactionsDetails> _repTrasacionDetails;
        private readonly IGenericRepository<ConfigurationSell> _repConfigurationSell;
        private readonly IGenericRepository<Concept> _repoConcept;
        private readonly IGenericRepository<ConfigurationPurchase> _repoConfigurationPurchase;
        private readonly IGenericRepository<Journal> _repJournals;
        private readonly IGenericRepository<JournaDetails> _repJournalsDetails;
        private readonly IGenericRepository<PaymentMethod> _repPaymentMethod;
        private readonly IGenericRepository<Contact> _repContacts;
        private readonly IGenericRepository<GroupTaxesTaxes> _repGroupTaxesTaxes;
        private readonly IGenericRepository<Taxes> _repTaxes;

        public TransactionService(IGenericRepository<Transactions> repTrasacion, IGenericRepository<Form> repForm,
            IGenericRepository<TransactionsDetails> repTrasacionDetails,
            IGenericRepository<ConfigurationSell> repConfigurationSell,
            IGenericRepository<ConfigurationPurchase> repoConfigurationPurchase,
            IGenericRepository<Journal> repJournals,
            IGenericRepository<JournaDetails> repJournalsDetails,
            IGenericRepository<Concept> repoConcept,
            IGenericRepository<PaymentMethod> repPaymentMethod, IGenericRepository<Contact> repContacts,
            IGenericRepository<GroupTaxesTaxes> repGroupTaxesTaxes, IGenericRepository<Taxes> repTaxes)
        {
            _repTrasacion = repTrasacion;
            _repForm = repForm;
            _repTrasacionDetails = repTrasacionDetails;
            _repConfigurationSell = repConfigurationSell;
            _repoConfigurationPurchase = repoConfigurationPurchase;
            _repJournals = repJournals;
            _repoConcept = repoConcept;
            _repJournalsDetails = repJournalsDetails;
            _repPaymentMethod = repPaymentMethod;
            _repContacts = repContacts;
            _repGroupTaxesTaxes = repGroupTaxesTaxes;
            _repTaxes = repTaxes;
        }

        private string GetSecuencie(int sequence, int input, string prex)
        {
            input++;
            string Out = "";
            int gol = sequence - input.ToString().Length;
            for (int i = 0; i < gol; i++)
            {
                Out += "0";
            }

            prex += Out + input.ToString();
            return prex;
        }


        public async Task<Transactions> TransactionProcess(Transactions transactions, Guid formId)
        {
            try
            {
                if (transactions.Id == Guid.Empty)
                {
                    var rowForm = await _repForm.GetById(formId);
                    if (rowForm.AllowSequence != null)
                    {
                        if (rowForm.AllowSequence.Value)
                        {
                            rowForm.Sequence = rowForm.Sequence.Value + 1;
                            transactions.Code = rowForm.Prefix + rowForm.Sequence.ToString();
                        }
                    }

                    await _repTrasacion.InsertAsync(transactions);

                    switch (transactions.TransactionsType)
                    {
                        case (int)Constants.Constants.Document.InvoiceCredit:
                            transactions.TransactionStatusId = Guid.Parse("85685D53-D6A6-4381-944B-965ED1147FBC");

                            await AccountingTransaction(TypeAccountingTransaction.SellLayaway, transactions);
                            await SecuenceNext(transactions);
                            break;
                        case (int)Constants.Constants.Document.InvoiceCash:
                            transactions.TransactionStatusId = Guid.Parse("85685D53-D6A6-4381-944B-965ED1187FBD");
                            await AccountingTransaction(TypeAccountingTransaction.Sell, transactions);
                            await SecuenceNext(transactions);

                            break;
                        case (int)Constants.Constants.Document.InvoceReturn:
                            transactions.TransactionStatusId = Guid.Parse("85685D53-D6A6-4381-944B-965ED1187FBA");
                            break;

                        case (int)Constants.Constants.Document.ExpenseCash:
                            transactions.TransactionStatusId = Guid.Parse("85685D53-D6A6-4381-944B-965ED1187FBD");
                            await AccountingTransaction(TypeAccountingTransaction.Purchase, transactions);


                            break;
                        case (int)Constants.Constants.Document.ExpenseCredit:
                            transactions.TransactionStatusId = Guid.Parse("85685D53-D6A6-4381-944B-965ED1147FBD");
                            await AccountingTransaction(TypeAccountingTransaction.PurchaseLayaway, transactions);


                            break;
                        case (int)Constants.Constants.Document.ExpenseOrders:
                            transactions.TransactionStatusId = Guid.Parse("85685D53-D6A6-4381-944B-995ED1667FBA");
                            await AccountingTransaction(TypeAccountingTransaction.PurchaseLayaway, transactions);

                            break;
                        case (int)Constants.Constants.Document.InvoceOrders:
                            transactions.TransactionStatusId = Guid.Parse("85685D53-D6A6-4381-944B-995ED1187FBA");
                            await AccountingTransaction(TypeAccountingTransaction.PurchaseLayaway, transactions);

                            break;
                        case (int)Constants.Constants.Document.InvoiceQuotes:
                            transactions.TransactionStatusId = Guid.Parse("85685D53-D6A6-4381-944B-995ED2967FBA");
                            await AccountingTransaction(TypeAccountingTransaction.PurchaseLayaway, transactions);

                            break;
                        case (int)Constants.Constants.Document.ExpenseQuates:
                            transactions.TransactionStatusId = Guid.Parse("85685D53-D6A6-4381-944B-995ED2167FBA");
                            await AccountingTransaction(TypeAccountingTransaction.PurchaseLayaway, transactions);

                            break;
                    }


                    await _repTrasacion.SaveChangesAsync();
                }
                else
                {
                    await SecuenceNext(transactions, false);
                    await _repTrasacion.Update(transactions);


                    var _TransantionDetealleForDelete = await _repTrasacionDetails
                        .Find(x => x.TransactionsId == transactions.Id).ToListAsync();


                    foreach (var item in _TransantionDetealleForDelete)
                    {
                        var resulDelte = await _repTrasacionDetails.Delete(item.Id);
                        await _repTrasacionDetails.SaveChangesAsync();
                    }

                    var TransactionsDetailsList = transactions.TransactionsDetails;
                    transactions.TransactionsDetails = TransactionsDetailsList;

                    await _repTrasacionDetails.InsertArray(transactions.TransactionsDetails);

                    await _repTrasacion.SaveChangesAsync();
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

        private async Task SecuenceNext(Transactions transactions, bool Sequence = true)
        {
            var resultContact = await _repContacts.Find(x => x.Id == transactions.ContactId)
                .Include(x => x.Numeration)
                .FirstOrDefaultAsync();
            if (resultContact is { NumerationId: { } })
            {
                if (resultContact.Numeration.Automatic)
                {
                    if (Sequence)
                    {
                        resultContact.Numeration.Sequence += 1;

                        transactions.TaxNumber =
                            resultContact.Numeration.Prefix + resultContact.Numeration.Sequence.ToString();
                    }

                    if (resultContact.Numeration.Pie_Invoice != null)
                        transactions.Commentary = resultContact.Numeration.Pie_Invoice;

                    decimal porceamount = 0;


                    var groupTaxes = await _repGroupTaxesTaxes
                        .Find(x => x.IsActive == true && x.GroupTaxesId == resultContact.TaxesId)
                        .Include(x => x.Taxes).ToListAsync();


                    foreach (var rowgroupTaxes in groupTaxes)
                    {
                        var rowTaxes = await _repTaxes.Find(x => x.IsActive == true && x.Id == rowgroupTaxes.TaxesId)
                            .FirstOrDefaultAsync();

                        porceamount += rowTaxes.Percentage;
                    }

                    decimal totalAmount = 0;

                    totalAmount = transactions.GlobalTotalTax * (porceamount / 100);
                    transactions.TotalTax = totalAmount;
                    transactions.GlobalTotalTax = transactions.GlobalTotalTax + transactions.TotalTax;
                    transactions.TotalAmount = transactions.GlobalTotal;
                    ;
                }
                else
                {
                    transactions.TotalTax = 0;
                    transactions.TotalAmount = transactions.GlobalTotal;

                    transactions.GlobalTotal = transactions.TotalAmount + transactions.TotalTax;
                }
            }
            else
            {
                transactions.TotalTax = 0;
                transactions.TotalAmount = transactions.GlobalTotal;

                transactions.GlobalTotal = transactions.TotalAmount + transactions.TotalTax;
            }
        }

        private enum TypeAccountingTransaction
        {
            SellLayaway = 0,
            Sell = 1,
            PurchaseLayaway = 10,
            Purchase = 11,
        }

        #region AccountingTransaccion

        private async Task<bool> AccountingTransaction(TypeAccountingTransaction typeAccountingTransaction,
            Transactions document)
        {
            bool Result = true;
            var ConfigurationSell = await _repConfigurationSell.FirstOrDefaultAsync();

            var ConfigurationPurchase = await _repoConfigurationPurchase.FirstOrDefaultAsync();
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
                        foreach (var TransactionsD in document.TransactionsDetails)
                        {
                            var RepoConcept = await _repoConcept.GetById(TransactionsD.ReferenceId);
                            if (RepoConcept != null)
                            {
                                if (RepoConcept.AccountSalesId.HasValue)
                                {
                                    JournaDetails journaDetailsConcept = NewJournaDetailsRow(
                                        journal.Id, RepoConcept.AccountSalesId.Value, 0, TransactionsD.Total);
                                    journaDetailsList.Add(journaDetailsConcept);
                                }

                                //Resta el inventario
                                decimal StockExist = RepoConcept.Stock.HasValue ? RepoConcept.Stock.Value : 0;
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

                        await _repJournals.InsertAsync(journal);
                    }

                    break;

                case TypeAccountingTransaction.Sell:

                    journal.Code = document.Code;

                    journal.Date = DateTime.Now;

                    journal.Reference = document.Reference;

                    var payment = _repPaymentMethod.Find(x => x.Id == document.PaymentMethodId).Include(x => x.Banks)
                        .FirstOrDefault();

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
                            var RepoConcept = await _repoConcept.GetById(TransactionsD.ReferenceId);
                            if (RepoConcept != null)
                            {
                                if (RepoConcept.AccountSalesId.HasValue)
                                {
                                    JournaDetails journaDetailsConcept = NewJournaDetailsRow(
                                        journal.Id, RepoConcept.AccountSalesId.Value, 0, TransactionsD.Total);
                                    journaDetailsList2.Add(journaDetailsConcept);
                                }

                                //Resta el inventario
                                decimal StockExist = RepoConcept.Stock.HasValue ? RepoConcept.Stock.Value : 0;
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

                        await _repJournals.InsertAsync(journal);
                    }

                    break;

                case TypeAccountingTransaction.PurchaseLayaway:

                    journal.Code = document.Code;

                    journal.Date = DateTime.Now;

                    journal.Reference = document.Reference;

                    journal.TypeRegisterId = document.Id;

                    var TransactionPurchaseLayawayGobalTotal =
                        await GetdocumentGlobalTotal(document.TransactionsDetails);

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
                            var RepoConcept = await _repoConcept.GetById(TransactionsD.ReferenceId);
                            if (RepoConcept != null)
                            {
                                if (RepoConcept.AccountCostId.HasValue)
                                {
                                    JournaDetails journaDetailsConcept = NewJournaDetailsRow(
                                        journal.Id, RepoConcept.AccountCostId.Value, TransactionsD.Total, 0);
                                    PurchaseLayawatDetailsList.Add(journaDetailsConcept);
                                }

                                //Resta el inventario
                                decimal StockExist = RepoConcept.Stock.HasValue ? RepoConcept.Stock.Value : 0;
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
                            journal.Id, ConfigurationPurchase.AccountPay.Value, 0,
                            TransactionPurchaseLayawayGobalTotal.Total);

                        PurchaseLayawatDetailsList.Add(journaDetails);
                    }

                    if (PurchaseLayawatDetailsList.Count > 0)
                    {
                        journal.JournaDetails = PurchaseLayawatDetailsList;

                        await _repJournals.InsertAsync(journal);
                    }

                    break;

                case TypeAccountingTransaction.Purchase:
                    var paymentnPurcha = await _repPaymentMethod.GetById(document.PaymentMethodId);

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
                            var RepoConcept = await _repoConcept.GetById(TransactionsD.ReferenceId);
                            if (RepoConcept != null)
                            {
                                if (RepoConcept.AccountCostId.HasValue)
                                {
                                    JournaDetails journaDetailsConcept = NewJournaDetailsRow(
                                        journal.Id, RepoConcept.AccountCostId.Value, TransactionsD.Total, 0);
                                    PurchaseDetailsList.Add(journaDetailsConcept);
                                }

                                //Resta el inventario
                                decimal StockExist = RepoConcept.Stock.HasValue ? RepoConcept.Stock.Value : 0;
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
                        if (ConfigurationPurchase.AccountTaxholding != null)
                        {
                            JournaDetails journaDetails = NewJournaDetailsRow(
                                journal.Id,
                                ConfigurationPurchase.AccountTaxholding.Value, TransactionPurchaselTotal.Tax, 0);

                            PurchaseDetailsList.Add(journaDetails);
                        }
                    }


                    if (TransactionPurchaselTotal.Total > 0)
                    {
                        if (paymentnPurcha.Banks != null)
                        {
                            if (paymentnPurcha.Banks.LedgerAccountId.HasValue)
                            {
                                JournaDetails journaDetails = NewJournaDetailsRow(
                                    journal.Id, paymentnPurcha.Banks.LedgerAccountId.Value, 0,
                                    TransactionPurchaselTotal.Total);

                                PurchaseDetailsList.Add(journaDetails);
                            }
                        }
                    }


                    if (PurchaseDetailsList.Count > 0)
                    {
                        journal.JournaDetails = PurchaseDetailsList;

                        await _repJournals.InsertAsync(journal);
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


        private async Task<TransactionsDetails> GetdocumentGlobalTotal(List<TransactionsDetails> document)
        {
            TransactionsDetails transactions = new TransactionsDetails();

            transactions.Total = document.Sum(x => x.Total);
            transactions.Tax = document.Sum(x => x.Tax);

            return transactions;
        }

        #endregion
    }
}