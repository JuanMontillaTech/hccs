using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class Taxes : Audit
    {
       
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
      
        public Guid? CreditLedgerAccountId { get; set; }
        public Guid? DebitLedgerAccountId { get; set; }
    }

    public class GroupTaxes : Audit
    {
        public string Description { get; set; }
       
    }



    public class GroupTaxesTaxes : Audit
    {
        [ForeignKey("GroupTaxes")]
        public Guid GroupTaxesId { get; set; }

        [ForeignKey("Taxes")]
        public Guid TaxesId { get; set; }
        public virtual Taxes Taxes { get; set; }
        public virtual GroupTaxes GroupTaxes { get; set; }

    }
}
