using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity;

public class TransaccionAccountForm: Audit
{
    
    [ForeignKey("Form")]
    public Guid FormId { get; set; }   
    [ForeignKey("LedgerAccount")]
    public Guid LedgerAccountId { get; set; }
    public int WayTransaccion { get; set; }
    public int TransaccionField { get; set; }
    public int Index { get; set; } = 0;
    public virtual Form Form { get; set; }
    public virtual LedgerAccount LedgerAccount { get; set; }
     

}