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
    public class FormRuleDto : AuditDto
    {
        [ForeignKey("Froms")]
        public Guid FormId { get; set; }
        public string Rule { get; set; }

        public virtual FormDto Froms { get; set; }

    }
}
