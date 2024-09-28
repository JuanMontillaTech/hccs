
using ERP.Domain.Command;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Dtos
{
    public class FormLedgerAccountDto : AuditDto
    {
        [ForeignKey("LedgerAccount")]
        public Guid LedgerAccountId { get; set; }

        public virtual LedgerAccountDto LedgerAccount { get; set; }

        [ForeignKey("Froms")]
        public Guid FormId { get; set; }
        public virtual FormDto Forms { get; set; }


    }
}
