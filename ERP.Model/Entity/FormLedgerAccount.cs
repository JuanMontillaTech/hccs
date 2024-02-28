using ERP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
