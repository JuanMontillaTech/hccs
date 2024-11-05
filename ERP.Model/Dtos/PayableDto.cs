using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos;

public class PayableDto   : AuditDto
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
    public virtual ContactDto  Contact { get; set; } 
    public virtual PaymentMethodDto PaymentMethod { get; set; }
}