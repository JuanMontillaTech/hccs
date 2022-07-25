using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class RollForm : Audit
    {
        [ForeignKey("Rolles")]
        public Guid? RollId { get; set; }


        [ForeignKey("Froms")]
        public Guid? FormId { get; set; }

        public virtual Roll Rolles { get; set; }
        public virtual Form Froms { get; set; }
    }
}

