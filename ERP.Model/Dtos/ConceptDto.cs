using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.AbstractEntiy;
using ERP.Domain.Command;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;

namespace ERP.Model.Dtos
{
    public class ConceptDto :AuditDto
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


        [ForeignKey("AccountReturnedList")]
        public Guid? AccountReturnedId { get; set; }

        [ForeignKey("AccountCommissionList")]
        public Guid? AccountCommissionId { get; set; }
        public decimal? PricePurchase { get; set; } = 0;
        public decimal? PriceSale { get; set; } = 0;
        public string Reference { get; set; }
        public bool? IsPurchase { get; set; }  
        public bool? ForSale { get; set; } 
        public bool? SellOutStock { get; set; }   
        public bool IsServicie { get; set; }  
        public decimal? Stock { get; set; } = 0;
        public virtual CatalogueDto  Catalogues { get; set; }
        public virtual LedgerAccountDto AccountInventoryList { get; set; }
        public virtual LedgerAccountDto AccountCostList { get; set; }
        public virtual LedgerAccountDto AccountSalesLis { get; set; }
        public virtual LedgerAccountDto AccountDiscountList { get; set; }
        public virtual LedgerAccountDto AccountReturnedList { get; set; }
        public virtual LedgerAccountDto AccountCommissionList { get; set; }
    }
 

}