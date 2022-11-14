using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class TransactionLocationTransaction : Audit
    {
        [ForeignKey("TransactionLocation")]
        public Guid TransactionLocationId { get; set; }
        [ForeignKey("Transactions")]
        public Guid TransactionId { get; set; }
        public virtual TransactionLocation TransactionLocation { get; set; }
        public virtual Transactions Transactions { get; set; }

    }
}
