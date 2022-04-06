using ERP.Domain.Command;

using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class TaxesDto
    { 
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public int TaxType { get; set; }
        public Guid? CreditLedgerAccountId { get; set; }
        public Guid? DebitLedgerAccountId { get; set; }
    }

    public class TaxesIdDto : AuditDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public int TaxType { get; set; }
        public Guid? CreditLedgerAccountId { get; set; }
        public Guid? DebitLedgerAccountId { get; set; }

    }

}
