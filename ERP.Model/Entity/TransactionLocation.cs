using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class TransactionLocation : Audit
    {
        public string Name { get; set; }
        public Guid TransactionFormId { get; set; }

    }
    public class TransactionLocationTransaction : Audit
    {
        [ForeignKey("TransactionLocation")]
        public Guid TransactionLocationId { get; set; }

        [ForeignKey("Transaction")]
        public Guid TransactionId { get; set; }
        public virtual Transactions Transaction { get; set; }
        public virtual TransactionLocation TransactionLocation { get; set; }

    }

}
