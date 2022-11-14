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
    public class TransactionLocationTransactionDto : AuditDto
    {
        [ForeignKey("TransactionLocation")]
        public Guid TransactionLocationId { get; set; }
        [ForeignKey("Transactions")]
        public Guid TransactionId { get; set; }
        public virtual TransactionLocationDto TransactionLocation { get; set; }
        public virtual TransactionsDto Transactions { get; set; }

    }
}
