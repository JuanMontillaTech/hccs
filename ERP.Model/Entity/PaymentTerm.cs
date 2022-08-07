using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class PaymentTerm : Audit
    {
        public string Name { get; set; }
        [ForeignKey("Banks")] public Guid BankId { get; set; }
        public virtual Banks Banks { get; set; }
        
        
        
    }
}
 

