using ERP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks; 
using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore; 

namespace ERP.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly IGenericRepository<Transactions> _repTrasacion;
        private readonly IGenericRepository<Form> _repForm;
        private readonly IGenericRepository<TransactionsDetails> _repTrasacionDetails;
        private readonly IGenericRepository<ConfigurationSell> _repConfigurationSell;
        private readonly IGenericRepository<Concept> _repoConcept;
        private readonly IGenericRepository<Box> _repoBox;
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
            IGenericRepository<GroupTaxesTaxes> repGroupTaxesTaxes, IGenericRepository<Taxes> repTaxes, IGenericRepository<Box> repoBox)
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
            _repoBox = repoBox;
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
                    await SecuenceNext(transactions);

                    await _repTrasacion.InsertAsync(transactions);

                    await SetJournalProsses(transactions);


                    await _repTrasacion.SaveChangesAsync();
                }
                else
                {
                   
                    await SecuenceNext(transactions,false);
                    await _repTrasacion.Update(transactions);
                    await SetJournalProsses(transactions);
                    await _repTrasacion.SaveChangesAsync();
                    var transactionIdsToUpdate = transactions.TransactionsDetails.Select(x => x.Id).ToList();
                    var transactionsToUpdate = await _repTrasacionDetails
                        .Find(x => x.TransactionsId == transactions.Id && transactionIdsToUpdate.Contains(x.Id) &&
                                   transactions.IsActive)
                        .ToListAsync();
                    foreach (var item in transactionsToUpdate)
                    {
                        item.IsActive = false;
                        await _repTrasacionDetails.Update(item);
                    }

                    //List<TransactionsDetails> transactionsDetailspdate =
                    //    transactions.TransactionsDetails.Where(t => t.Id != Guid.Empty).ToList();
                    //if (transactionsDetailspdate.Count() > 0)
                    //{
                    //    await _repTrasacionDetails.UpdateArray(transactionsDetailspdate);
                    //    await _repTrasacionDetails.SaveChangesAsync();
                    //}
                    List<TransactionsDetails> trantraccionDetalleisInsert =
                        transactions.TransactionsDetails.Where(t => t.Id == Guid.Empty).ToList();
                    await _repTrasacionDetails.InsertArray(trantraccionDetalleisInsert);
                    await _repTrasacionDetails.SaveChangesAsync();
                    
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

        private async Task SetJournalProsses(Transactions transactions)
        {
            switch (transactions.TransactionsType)
            {
                case (int)Constants.Constants.Document.InvoiceCredit:
                    transactions.TransactionStatusId = Guid.Parse("85685D53-D6A6-4381-944B-965ED1147FBC");

                    await AccountingTransaction(TypeAccountingTransaction.SellLayaway, transactions);
                    
                    break;
                case (int)Constants.Constants.Document.InvoiceCash:

                    await AccountingTransaction(TypeAccountingTransaction.Sell, transactions);
                   

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
                    transactions.TotalAmountTax = transactions.GlobalTotalTax;
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
            switch (typeAccountingTransaction)
            {
                case TypeAccountingTransaction.SellLayaway:

                  
                     
                    break;

                case TypeAccountingTransaction.Sell:      
                  
                    foreach (var  documentTransactionsDetail in document.TransactionsDetails)
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
                            ammount, 0);

                        journaDetailsList.Add(journaDebit); 
                        
                        var concept = await _repoConcept.GetById(documentTransactionsDetail.ReferenceId.Value);

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
             

                

                    break;

                case TypeAccountingTransaction.PurchaseLayaway:

                
                     
                    break;

                case TypeAccountingTransaction.Purchase:
                    foreach (var  documentTransactionsDetail in document.TransactionsDetails)
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
                            journal.Id, accountSellId, ammount,0 );
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