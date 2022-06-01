using ERP.Domain.Command;

using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class TransactionReceiptDto : AuditDto
    {  
        public Guid? TransactionsId { get; set; }
        public decimal Paid { get; set; }
        public decimal ForPaid { get; set; }
        public decimal Received { get; set; }
        public virtual TransactionsDto Transactions { get; set; }
    }

    public class TransactionReceiptDetailsDto : AuditDto
    {
        public Guid? TransactionsId { get; set; }

        public string Code { get; set; }
        public decimal Paid { get; set; }
        public decimal ForPaid { get; set; }
        public decimal Received { get; set; }
        public virtual TransactionsDto Transactions { get; set; }
    }
}
