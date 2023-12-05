using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity;

public class Box  : Audit
{

    public string Name { get; set; }
    
    [ForeignKey("LedgerAccount")]
    public Guid? LedgerAccountId { get; set; }
    
    public virtual LedgerAccount LedgerAccount { get; set; }
    
    [ForeignKey("Currency")]
    public Guid? CurrencyId { get; set; }
    
    public virtual LedgerAccount Currency { get; set; }
    
    
}