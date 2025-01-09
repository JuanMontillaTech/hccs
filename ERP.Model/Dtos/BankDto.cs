
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class BankDto :AuditDto
    {
        [ForeignKey("Currencys")]
        public Guid? CurrencyId { get; set; }

        [Required(ErrorMessage = "El nombre del banco es requerido.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El número de cuenta es requerido.")]
        public string AccountNumber { get; set; }

        [ForeignKey("LedgerAccount")]
        public Guid? LedgerAccountId { get; set; }

    }

    public class BankDetallisDto :AuditDto
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
