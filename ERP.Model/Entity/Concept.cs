using ERP.Domain.AbstractEntiy;
 
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class Concept : Audit 
    {
        public string Description { get; set; }
        [ForeignKey("Catalogues")]
        public Guid? CatalogueId { get; set; }
        [ForeignKey("AccountInventoryList")]
        public Guid? AccountInventoryId { get; set; }

        [ForeignKey("AccountCostList")]
        public Guid? AccountCostId { get; set; }


        [ForeignKey("AccountSalesLis")]
        public Guid? AccountSalesId { get; set; }


        [ForeignKey("AccountDiscountList")]
        public Guid? AccountDiscountId { get; set; }
        [ForeignKey("GroupTaxesTaxes")]
        public Guid? GroupTaxesTaxesId { get; set; }

        public virtual GroupTaxesTaxes  GroupTaxesTaxes { get; set; }

        [ForeignKey("AccountReturnedList")]
        public Guid? AccountReturnedId { get; set; }

        [ForeignKey("AccountCommissionList")]
        public Guid? AccountCommissionId { get; set; }
        public decimal? PricePurchase { get; set; } = 0;
        public decimal? PriceSale { get; set; } = 0;
        public string Reference { get; set; }
        public bool? IsPurchase { get; set; } = false;
        public bool? IsExempt { get; set; } = false;
        public bool? ForSale { get; set; } = true;
        public bool? SellOutStock { get; set; } = true;
        public bool? IsServicie { get; set; } = false;
        public decimal? Stock { get; set; } = 0;
        public virtual Catalogue Catalogues { get; set; } 
        public virtual LedgerAccount AccountInventoryList { get; set; }
        public virtual LedgerAccount AccountCostList { get; set; }
        public virtual LedgerAccount AccountSalesLis { get; set; }
        public virtual LedgerAccount AccountDiscountList { get; set; }
        public virtual LedgerAccount AccountReturnedList { get; set; }
        public virtual LedgerAccount AccountCommissionList { get; set; }
    }
}
