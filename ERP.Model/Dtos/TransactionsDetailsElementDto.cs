using ERP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class TransactionsDetailsElementDto
    {
        public string Detaills { get; set; }

        [ForeignKey("TransactionsDetailsElementType")]
        public Guid? TransactionsDetailsElementTypeId { get; set; }

        public virtual TransactionsDetailsElementTypeDto TransactionsDetailsElementType { get; set; }
    }
}
