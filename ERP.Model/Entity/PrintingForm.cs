using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class PrintingForm : Audit
    {
        [ForeignKey("Froms")]
        public Guid? FormId { get; set; }

        public virtual Form Forms { get; set; }

        [ForeignKey("PrintingTemplates")]
        public Guid? PrintingTemplateId { get; set; }

        public virtual PrintingTemplate PrintingTemplates { get; set; }
    }
}
