
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Dtos
{
    public class BankDto
    {
        [ForeignKey("Currencys")]
        public Guid? CurrencyId { get; set; }

        public string Name { get; set; }

        public string AccountNumber { get; set; }

        [ForeignKey("LedgerAccount")]
        public Guid? LedgerAccountId { get; set; }
       
    }

    public class BankDetallisDto
    {
        [ForeignKey("Currencys")]
        public Guid? CurrencyId { get; set; }

        public string Name { get; set; }

        public string AccountNumber { get; set; }

        [ForeignKey("LedgerAccount")]
        public Guid? LedgerAccountId { get; set; }
        public virtual CurrencyDto Currencys { get; set; }
        public virtual LedgerAccountDto LedgerAccount { get; set; }
    }
}
