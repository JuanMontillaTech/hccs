using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class TransactionsDetailsElement : Audit
    {
        public string Detaills { get; set; }
      
        [ForeignKey("TransactionsDetailsElementType")]
        public Guid? TransactionsDetailsElementTypeId { get; set; }

        public virtual TransactionsDetailsElementType TransactionsDetailsElementType { get; set; }
    }
}
