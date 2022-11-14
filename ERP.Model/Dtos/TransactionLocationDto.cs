using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class TransactionLocationDto :AuditDto
    {
        [ForeignKey("Form")]
        public Guid TransactionFormId { get; set; }
        public string Name { get; set; }

        public virtual FormDto Form { get; set; }
    }
}
