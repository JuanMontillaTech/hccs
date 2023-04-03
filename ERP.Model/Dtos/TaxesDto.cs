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
    public class GroupTaxesDto : AuditDto
    {
        public string Description { get; set; }

    }


   
    public class GroupTaxesTaxesDto : AuditDto
    {
        [ForeignKey("GroupTaxes")]
        public Guid GroupTaxesId { get; set; }

        [ForeignKey("Taxes")]
        public Guid TaxesId { get; set; }
        public virtual TaxesDto Taxes { get; set; }
        public virtual GroupTaxesDto GroupTaxes { get; set; }
        public virtual string GroupName { get; set; }

    }


}
