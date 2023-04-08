using System;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos;

public class RecipePayDto : AuditDto
{
    public DateTime Date { get; set; }
    public Guid CurrencyId { get; set; }
    public Guid BankId { get; set; }
    public string Reference { get; set; }
    public Guid PaymentMethodId { get; set; }
    public string Code { get; set; }
    public decimal Pay { get; set; }
    
    public decimal GlobalTotal { get; set; }
    public Guid TransationId { get; set; }

}
    
    
 