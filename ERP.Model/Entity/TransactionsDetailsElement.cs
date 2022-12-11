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
        [ForeignKey("TransactionsDetails")]
        public Guid TransactionsDetailsId { get; set; }
        public Guid ReferenceId { get; set; }
        public string Detaills { get; set; }
      
        [ForeignKey("TransactionsDetailsElementType")]
        public Guid? TransactionsDetailsElementTypeId { get; set; }

        public virtual TransactionsDetailsElementType TransactionsDetailsElementType { get; set; }
        public virtual TransactionsDetails TransactionsDetails { get; set; }
    }
}
