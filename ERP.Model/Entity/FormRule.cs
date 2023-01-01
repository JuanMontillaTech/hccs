using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class FormRule : Audit
    {
        [ForeignKey("Froms")]
        public Guid FormId { get; set; }
        public string Rule { get; set; }

        public virtual Form Froms { get; set; }

    }
}
