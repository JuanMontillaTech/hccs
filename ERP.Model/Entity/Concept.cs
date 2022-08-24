using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class Concept : Audit
    {
        public string Description { get; set; }
     
        public decimal PricePurchase { get; set; }
        public decimal PriceSale { get; set; }
        public string Reference { get; set; }
        public bool IsPurchase { get; set; }
        public bool ForSale { get; set; }
        
        [ForeignKey("AccountInventoryList")]
        public Guid? AccountInventoryId{ get; set; }
        public virtual LedgerAccount AccountInventoryList { get; set; }
        
        [ForeignKey("AccountCostList")]
        public Guid? AccountCostId{ get; set; } 
        public virtual LedgerAccount AccountCostList { get; set; } 
        
        [ForeignKey("AccountSalesLis")]
        public Guid? AccountSalesId{ get; set; }
        public virtual LedgerAccount AccountSalesLis { get; set; }
        
        
        [ForeignKey("AccountDiscountList")]
        public Guid? AccountDiscountId{ get; set; }
        public virtual LedgerAccount AccountDiscountList { get; set; }
        
        
        [ForeignKey("AccountReturnedList")]
        public Guid? AccountReturnedId{ get; set; }
        public virtual LedgerAccount AccountReturnedList { get; set; }
        
        [ForeignKey("AccountCommissionList")]
        public Guid? AccountCommissionId{ get; set; }
        public virtual LedgerAccount AccountCommissionList { get; set; } 

    }
}
