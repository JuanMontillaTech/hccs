using ERP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class BankTransaction : Audit
    {
  
        public DateTime TransactionDate { get; set; }

        [ForeignKey("Bank")]
        public Guid BankId { get; set; }
 
        [ForeignKey("TransactionType")]
        public Guid TransactionTypeId { get; set; }

        [ForeignKey("PaymentMethod")]
        public Guid PaymentMethodId { get; set; }
        public decimal CashAmount { get; set; }
        public decimal CheckNumber { get; set; }
        public decimal Wiretransfer { get; set; } 
        
        public virtual TransactionType TransactionType { get; set; }
        public virtual Banks Bank { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }


    }
}
