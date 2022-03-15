using System; 

namespace ERP.Domain.Entity
{
    public class Concept : Audit
    { 
        public string Description { get; set; }
         public Guid? CreditLedgerAccountId { get; set; }
         public Guid? DebitLedgerAccountId { get; set; }

    }
}
