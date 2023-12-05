using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;
using ERP.Domain.Entity;

namespace ERP.Domain.Dtos;

public class BoxDto : AuditDto
{
    public string Name { get; set; }
    
    [ForeignKey("LedgerAccount")]
    public Guid? LedgerAccountId { get; set; }
    
    public virtual LedgerAccount LedgerAccount { get; set; }
    
    [ForeignKey("Currency")]
    public Guid? CurrencyId { get; set; }
    
    public virtual LedgerAccount Currency { get; set; }
}