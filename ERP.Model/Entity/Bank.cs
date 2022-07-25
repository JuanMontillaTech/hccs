using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class Banks : Audit
    {
        [ForeignKey("Currencys")]
        public Guid? CurrencyId { get; set; }

        public string Name { get; set; }

        public string AccountNumber { get; set; }  
     
        [ForeignKey("LedgerAccount")]
        public Guid? LedgerAccountId { get; set; }

        public virtual Currency Currencys { get; set; }

        public virtual LedgerAccount LedgerAccount { get; set; }
    }
}
