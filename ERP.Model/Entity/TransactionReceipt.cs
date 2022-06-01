using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Entity
{
    public class TransactionReceipt : Audit
    {

        [ForeignKey("Transactions")]
        public Guid? TransactionsId { get; set; }
        public decimal Paid { get; set; }
        public decimal ForPaid { get; set; }
        public decimal Received { get; set; }
        public virtual Transactions Transactions { get; set; }


    }
}
