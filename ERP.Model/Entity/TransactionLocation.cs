using ERP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class TransactionLocation : Audit
    {
        [ForeignKey("Form")]
        public Guid TransactionFormId { get; set; }
        public string Name { get; set; }

        public virtual Form Form { get; set; }
    }
}
