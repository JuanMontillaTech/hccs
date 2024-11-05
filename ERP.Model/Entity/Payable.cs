using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity;

public class Payable :  Audit
{
    
    public string AccountType { get; set; } 
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string DocumentNumber { get; set; }
    [ForeignKey("Contact")]
    public Guid CustomerSupplierId { get; set; } 
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }  
    [ForeignKey("PaymentMethod")]
    public Guid paymentMethodId { get; set; }  
    public virtual Contact  Contact { get; set; } 
   public virtual PaymentMethod PaymentMethod { get; set; }
}