using ERP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
   

    public class TransactionLocationDto : Audit
    {
        public string Name { get; set; }
        public Guid TransactionFormId { get; set; }

    }
    public class TransactionLocationTransactionDto : Audit
    {

        [ForeignKey("TransactionLocation")]
        public Guid TransactionLocationId { get; set; }

        [ForeignKey("Transaction")]
        public Guid TransactionId { get; set; }
        public virtual TransactionsDto Transaction { get; set; }
        public virtual TransactionLocationDto TransactionLocation { get; set; }

    }
}
