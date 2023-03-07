
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP.Domain.Entity
{
    public class TransactionReceipt : Audit
    {
        [ForeignKey("Transactions")]
  
        public string Document { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        
        [ForeignKey("Banks")]
        public Guid? BankId { get; set; }

        [ForeignKey("Contact")]
        public Guid? ContactId { get; set; }

        [ForeignKey("PaymentMethods")]
        public Guid? PaymentMethodId { get; set; }

        [ForeignKey("Currency")]
        public Guid? CurrencyId { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Banks Banks { get; set; }
        public virtual PaymentMethod PaymentMethods { get; set; }
   


    }
    public class TransactionReceiptDetails : Audit
    {
        public Guid TransactionReceiptId { get; set; }
        [ForeignKey("Transactions")]
        public Guid? TransactionsId { get; set; }    
        public decimal Paid { get; set; }
        public virtual Transactions Transactions { get; set; }
    }
}
