using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Entity
{
   public class LedgerAccount : Audit
    {
        public Guid? Belongs { get; set; }
        public string Name { get; set; }
        public string Code { get; set; } 
        public int? Nature { get; set; } 
        public int? LocationStatusResult { get; set; }
        public int? EntidadId { get; set; } 
    }

    public class LedgerAccountwihtBalance
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? LedgerAccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Debitor { get; set; }
        public decimal Creditor { get; set; } 
        public decimal Balance { get; set; }
        public int Origen { get; set; }

    }

}
