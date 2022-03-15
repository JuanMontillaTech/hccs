using System;
using ERP.Domain.Command;

namespace ERP.Model.Dtos
{
    public class ConceptDto
    {
        public string Description { get; set; }
        public Guid? CreditLedgerAccountId { get; set; }
        public Guid? DebitLedgerAccountId { get; set; }

    }

    public class ConceptIdDto : AuditDto
    {
        public string Description { get; set; }
        public Guid? CreditLedgerAccountId { get; set; }
        public Guid? DebitLedgerAccountId { get; set; }

    }

}