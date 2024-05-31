using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using ERP.Services.Interfaces;

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
        private readonly IGenericRepository<TransactionReceipt> _repTransactionReceipt;
        private readonly IGenericRepository<TransactionReceiptDetails> _repTransactionReceiptDetails;

        private readonly INumerationHelper _numerationHelper;
        private readonly IAccountingProcess _accountingProcess;

        public TransactionService(IGenericRepository<Transactions> repTrasacion, IGenericRepository<Form> repForm,
            IGenericRepository<TransactionsDetails> repTrasacionDetails,
            IGenericRepository<ConfigurationSell> repConfigurationSell,
            IGenericRepository<ConfigurationPurchase> repoConfigurationPurchase,
            IGenericRepository<Journal> repJournals,
            IGenericRepository<JournaDetails> repJournalsDetails,
            IGenericRepository<Concept> repoConcept,
            IGenericRepository<TransactionReceipt> repTransactionReceipt,
            IAccountingProcess accountingProcess,
            INumerationHelper numerationHelper,
            IGenericRepository<PaymentMethod> repPaymentMethod, IGenericRepository<Contact> repContacts,
            IGenericRepository<GroupTaxesTaxes> repGroupTaxesTaxes, IGenericRepository<Taxes> repTaxes, IGenericRepository<Box> repoBox, IGenericRepository<TransactionReceiptDetails> repTransactionReceiptDetails)
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
            _repTransactionReceiptDetails = repTransactionReceiptDetails;
            _repTransactionReceipt = repTransactionReceipt;
            _numerationHelper = numerationHelper;
            _accountingProcess = accountingProcess;
        }



        public async Task<Transactions> TransactionProcess(Transactions transactions, Guid formId)
        {
            try
            {   
                var contact = await GetContactWithNumeration(transactions.ContactId);

                var taxes = await GetTaxesForContact(contact); 
                // pregunto si tiene id de lo contrario le creo una nueva
                if (transactions.Id == Guid.Empty)
                {

                    transactions.Code = await _numerationHelper.GetNextNumerationSequence(formId);

                    transactions.TaxNumber = contact?.Numeration != null ? await _numerationHelper.ValidateAndFetchNextTaxNumber(transactions.ContactId) : null;

                    await CalculateTotalTax(transactions, taxes);

                    await _repTrasacion.InsertAsync(transactions);

                    await _accountingProcess.PostJournalEntry(transactions);

                    await _repTrasacion.SaveChangesAsync();
                }
                else
                {

                    await CalculateTotalTax(transactions, taxes);

                    await _repTrasacion.Update(transactions);

                   // await _accountingProcess.UpdateJournalEntry(transactions);

                    await _repTrasacion.SaveChangesAsync();

                    var transactionIdsToUpdate = transactions.TransactionsDetails.Select(x => x.Id).ToList();

                    var transactionsToUpdate = await _repTrasacionDetails
                        .Find(x => x.TransactionsId == transactions.Id && !transactionIdsToUpdate.Contains(x.Id) &&
                                   transactions.IsActive)
                        .ToListAsync();

                    foreach (var item in transactionsToUpdate)
                    {
                        item.IsActive = false;
                        await _repTrasacionDetails.Update(item);
                    }

                    var transactionDetailsToInsert = transactions.TransactionsDetails.Where(t => t.Id == Guid.Empty).ToList();
                    var transactionDetailsToUpdate = transactions.TransactionsDetails.Except(transactionDetailsToInsert).ToList();

                    if (transactionDetailsToInsert.Any())
                    {
                        await _repTrasacionDetails.InsertArray(transactionDetailsToInsert);
                        await _repTrasacionDetails.SaveChangesAsync();
                    }

                    if (transactionDetailsToUpdate.Any())
                    {
                        await _repTrasacionDetails.UpdateArray(transactionDetailsToUpdate);
                        await _repTrasacionDetails.SaveChangesAsync();
                    }
                }

                return transactions;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return null;
              
            }
        }

        private async Task CalculateTotalTax(Transactions transactions, List<GroupTaxesTaxes> taxes)
        {
            decimal totalTaxPercentage = await CalculateTotalTaxPercentage(taxes);

            decimal totalAmountTax = transactions.GlobalTotalTax * (totalTaxPercentage / 100);

            transactions.TotalTax = totalAmountTax;

            transactions.TotalAmountTax = transactions.GlobalTotalTax;

            transactions.GlobalTotalTax = transactions.GlobalTotalTax + transactions.TotalTax;

            transactions.TotalAmount = transactions.GlobalTotal;
        }

        private async Task<List<GroupTaxesTaxes>> GetTaxesForContact(Contact contact)
        {
            if (contact == null) return null;
            return await _repGroupTaxesTaxes
                .Find(x => x.IsActive && x.GroupTaxesId == contact.TaxesId)
                .Include(x => x.Taxes)
                .ToListAsync();
        }

        private async Task<Contact> GetContactWithNumeration(Guid? contactId)
        {
            return await _repContacts.Find(x => x.Id == contactId)
                .Include(x => x.Numeration)
                .FirstOrDefaultAsync();
        }
        private async Task<decimal> CalculateTotalTaxPercentage(List<GroupTaxesTaxes> taxes)
        {
            decimal totalTaxPercentage = 0;
            foreach (var groupTax in taxes)
            {
                var tax = await _repTaxes.Find(x => x.IsActive && x.Id == groupTax.TaxesId)
                    .FirstOrDefaultAsync();
                totalTaxPercentage += tax.Percentage;
            }
            return totalTaxPercentage;
        }

        private async Task<RecipePayDto> TransactionReceiptProcessCreate(RecipePayDto RecipePayDto)
        {
            TransactionReceipt transactionReceipt = new TransactionReceipt();
            var rowForm = await _repForm.GetById(RecipePayDto.FormId);
            if (rowForm.AllowSequence != null)
            {
                if (rowForm.AllowSequence.Value)
                {
                    rowForm.Sequence = rowForm.Sequence.Value + 1;
                    transactionReceipt.Document = rowForm.Prefix + rowForm.Sequence.ToString();
                }
            }

            transactionReceipt.BoxId = RecipePayDto.BoxId;
            transactionReceipt.ContactId = RecipePayDto.ContactId;
            transactionReceipt.Date = RecipePayDto.Date;
            transactionReceipt.Reference = RecipePayDto.Reference;
            transactionReceipt.PaymentMethodId = RecipePayDto.PaymentMethodId;
            transactionReceipt.CurrencyId = RecipePayDto.CurrencyId;
            transactionReceipt.Type = RecipePayDto.Type;
            List<TransactionReceiptDetails> transactionReceiptDetails = new List<TransactionReceiptDetails>();

            foreach (var item in RecipePayDto.RecipeDetalles)
            {
                var newtsR = new TransactionReceiptDetails()
                {
                    TransactionReceiptId = transactionReceipt.Id,
                    Paid = (decimal)item.Value,
                    referenceId = item.referenceId

                };
                transactionReceipt.Total += newtsR.Paid;
                transactionReceiptDetails.Add(newtsR);


            }

            transactionReceipt.TransactionReceiptDetails = transactionReceiptDetails;
            await _repTransactionReceipt.InsertAsync(transactionReceipt);
            await _repTransactionReceipt.SaveChangesAsync();

            return RecipePayDto;

        }

        private async Task<RecipePayDto> TransactionReceiptProcessUpdate(RecipePayDto RecipePayDto)
        {
            TransactionReceipt transactionReceipt = await _repTransactionReceipt.GetById(RecipePayDto.Id);


            transactionReceipt.BoxId = RecipePayDto.BoxId;
            transactionReceipt.ContactId = RecipePayDto.ContactId;
            transactionReceipt.Date = RecipePayDto.Date;
            transactionReceipt.Reference = RecipePayDto.Reference;
            transactionReceipt.PaymentMethodId = RecipePayDto.PaymentMethodId;
            transactionReceipt.CurrencyId = RecipePayDto.CurrencyId;
            List<TransactionReceiptDetails> transactionReceiptDetails = new List<TransactionReceiptDetails>();
            transactionReceipt.Total = 0;
            foreach (var item in RecipePayDto.RecipeDetalles)
            {

                if (item.Id == null)
                {
                    var newtsR = new TransactionReceiptDetails()
                    {
                        TransactionReceiptId = transactionReceipt.Id,
                        Paid = (decimal)item.Value,
                        referenceId = item.referenceId

                    };
                    transactionReceipt.Total += newtsR.Paid;
                    transactionReceiptDetails.Add(newtsR);
                }
                else
                {
                    var updateTrsD = await _repTransactionReceiptDetails.GetById(item.Id);

                    updateTrsD.Paid = (decimal)item.Value;
                    transactionReceipt.Total += updateTrsD.Paid;
                }




            }

            if (transactionReceiptDetails.Count >= 1)
                transactionReceipt.TransactionReceiptDetails = transactionReceiptDetails;

            await _repTransactionReceipt.Update(transactionReceipt);

            await _repTransactionReceipt.SaveChangesAsync();

            return RecipePayDto;

        }


        public async Task<RecipePayDto> TransactionReceiptProcess(RecipePayDto RecipePayDto)
        {


            if (RecipePayDto.Id == null)
            {
                return await TransactionReceiptProcessCreate(RecipePayDto);


            }
            else
            {
                return await TransactionReceiptProcessUpdate(RecipePayDto);

            }

        }




        #region AccountingTransaccion






        #endregion
    }
}