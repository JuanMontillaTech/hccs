
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP.Domain.Entity
{
    public   class TransactionReceipt : Audit
    {
      
        public string Document { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        
        [ForeignKey("Box")]
        public Guid BoxId { get; set; }

        [ForeignKey("Contact")]
        public Guid ContactId { get; set; }  
        
        [ForeignKey("RecipeStatus")]
        public Guid RecipeStatusId { get; set; }

        [ForeignKey("PaymentMethods")]
        public Guid PaymentMethodId { get; set; }

        [ForeignKey("Currency")]
        public Guid CurrencyId { get; set; }
        public Contact Contact { get; set; }
        public Box Box { get; set; }
        public PaymentMethod PaymentMethods { get; set; }
        public Currency Currency { get; set; }  
        public int Type { get; set; }
        public decimal Total { get; set; }
        public  List<TransactionReceiptDetails>TransactionReceiptDetails {  get; set; } 
        
        public  virtual RecipeStatus RecipeStatus {  get; set; } 
    }
    public class TransactionReceiptDetails : Audit
    {
        [ForeignKey("TransactionReceipt")]
        public Guid TransactionReceiptId { get; set; }
           
        public decimal Paid { get; set; }
        
        public Guid referenceId { get; set; } 
        public virtual TransactionReceipt TransactionReceipt { get; set; }
    }
}
