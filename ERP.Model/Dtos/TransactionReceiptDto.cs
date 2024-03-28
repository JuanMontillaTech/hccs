using ERP.Domain.Command;
using ERP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class TransactionReceiptDto : AuditDto
    {
        public string Document { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        [ForeignKey("Box")]
        public Guid BoxId { get; set; }
        [ForeignKey("Contact")]
        public Guid? ContactId { get; set; }
        [ForeignKey("PaymentMethods")]
        public Guid? PaymentMethodId { get; set; }
        [ForeignKey("Currency")]
        public Guid? CurrencyId { get; set; } 
        public decimal Total { get; set; }    
        public virtual ContactDto Contact { get; set; }
        public virtual CurrencyDto Currency { get; set; }
        public virtual BoxDto Box { get; set; }
        public virtual PaymentMethodDto PaymentMethods { get; set; }
       
        public int Type { get; set; }
 
        public List<TransactionReceiptDetailsDto>  TransactionReceiptDetails { get; set; }
    }
    public class TransactionReceiptOutDto  
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAdress { get; set; }
        public string TaxId { get; set; }
        public string CompanyPhones { get; set; }
        public string TaxContactNumber { get; set; }
        public string TaxNumber { get; set; }
        public  TransactionReceiptDto TransactionReceipt { get; set; }
        public List<TransactionReceiptDetailsDto>  TransactionReceiptDetails { get; set; }
    }
    public class TransactionReceiptDetailsDto : AuditDto
    {
      

        
        public Guid TransactionReceiptId { get; set; }
           
        public decimal Paid { get; set; }
        
        public Guid referenceId { get; set; } 
   


    }
}
 