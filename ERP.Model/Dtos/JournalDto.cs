using ERP.Domain.Command;
using ERP.Domain.Entity;
using System;
using System.Collections.Generic;

public class JournalDto : AuditDto
{
    public Guid? Id { get; set; }
    public string Code { get; set; }
    public string Reference { get; set; }
    public string Commentary { get; set; }
    public DateTime Date { get; set; }
    public Guid? TypeRegisterId { get; set; }
    public virtual ICollection<JournaDetailsDto> JournaDetails { get; set; }

}
public class JournalIdDto : AuditDto
{
    public Guid? ContactId { get; set; }
    public Guid? JournalId { get; set; }
    public Guid? LedgerAccountId { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string Commentary { get; set; }
}
public class JournaDetailsDto : AuditDto
{

    public Guid? ContactId { get; set; }
    public Guid? JournalId { get; set; }

    public Guid? LedgerAccountId { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string Commentary { get; set; } 
}

