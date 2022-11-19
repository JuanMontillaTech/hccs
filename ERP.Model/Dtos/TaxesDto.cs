using ERP.Domain.Command;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class TaxesDto : AuditDto
    { 
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
       
        [ForeignKey("CreditLedger")]
        public Guid? CreditLedgerAccountId { get; set; } 
        [ForeignKey("DebitLedger")]
        public Guid? DebitLedgerAccountId { get; set; }  
        public virtual LedgerAccountDto DebitLedger { get; set; }
        public virtual LedgerAccountDto CreditLedger { get; set; }
    }

    
}
