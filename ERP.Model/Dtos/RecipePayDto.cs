using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos;

public class RecipePayDto : AuditDto
{
    public DateTime Date { get; set; }
    public Guid CurrencyId { get; set; }
    public Guid BoxId { get; set; }
    public Guid FormId { get; set; }
    public Guid ContactId { get; set; }
    public string Reference { get; set; }
    public Guid PaymentMethodId { get; set; }
    public string Code { get; set; }
    public decimal Pay { get; set; } 
    public int Type { get; set; } 
    public decimal GlobalTotal { get; set; }
    public Guid? TransationId { get; set; } 
    [ForeignKey("RecipeStatus")]
    public Guid RecipeStatusId { get; set; }
    public virtual List<RecipeDetalles> RecipeDetalles { get; set; }
    public virtual List<RecipeStatusDto> RecipeStatus { get; set; }

}

public class RecipeDetalles
{

    public Guid? Id { get; set; }
    public Guid referenceId { get; set; }
    public string Label { get; set; }
    public decimal? Value { get; set; }
}
    
    
 