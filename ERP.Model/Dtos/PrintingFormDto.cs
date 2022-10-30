using ERP.Domain.Command;
using ERP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class PrintingFormDto : AuditDto
    {
        [ForeignKey("Froms")]
        public Guid? FormId { get; set; }

        public virtual FormDto Forms { get; set; }

        [ForeignKey("PrintingTemplates")]
        public Guid? PrintingTemplateId { get; set; }

        public virtual ReportQueryDto PrintingTemplates { get; set; }
    }
}
