
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP.Domain.Entity
{
    public sealed class TransactionReceipt : Audit
    {
       
        public string Document { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        
        [ForeignKey("Banks")]
        public Guid BankId { get; set; }

        [ForeignKey("Contact")]
        public Guid ContactId { get; set; }

        [ForeignKey("PaymentMethods")]
        public Guid PaymentMethodId { get; set; }

        [ForeignKey("Currency")]
        public Guid CurrencyId { get; set; }
        public Contact Contact { get; set; }
        public Banks Banks { get; set; }
        public PaymentMethod PaymentMethods { get; set; }
        public Currency Currency { get; set; }
        public TransactionReceiptDetails TransactionReceiptDetail {  get; set; }


    }
    public class TransactionReceiptDetails : Audit
    {
        [ForeignKey("TransactionReceipt")]
        public Guid TransactionReceiptId { get; set; }
        [ForeignKey("Transactions")]
        public Guid? TransactionsId { get; set; }    
        public decimal Paid { get; set; }
        public virtual Transactions Transactions { get; set; }
        public virtual TransactionReceipt TransactionReceipt { get; set; }
    }
}
