using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class FormLedgerAccount : Audit
    {
        [ForeignKey("LedgerAccount")]
        public Guid LedgerAccountId { get; set; }

        public virtual LedgerAccount LedgerAccount { get; set; }

        [ForeignKey("Froms")]
        public Guid FormId { get; set; }
        public virtual Form Forms { get; set; }

        public int? Index { get; set; } 
    }
}
