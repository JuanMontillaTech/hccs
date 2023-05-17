using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;
using ERP.Domain.Entity;

namespace ERP.Domain.Dtos;

public class TransaccionAccountFormDto : AuditDto
{
    [ForeignKey("Form")]
    public Guid FormId { get; set; }   
    [ForeignKey("LedgerAccount")]
    public Guid LedgerAccountId { get; set; }
    public int WayTransaccion { get; set; }
    public int TransaccionField { get; set; }
    public int Index { get; set; } = 0;
    public virtual FormDto Form { get; set; }
    public virtual LedgerAccountDto LedgerAccount { get; set; }

}