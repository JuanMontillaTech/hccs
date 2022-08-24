using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Model.Dtos
{
    public class ConceptDto
    {
        public string Description { get; set; }
        [ForeignKey("AccountInventoryList")]
        public Guid? AccountInventoryId{ get; set; }
        public virtual LedgerAccountDto AccountInventoryList { get; set; }
        
        [ForeignKey("AccountCostList")]
        public Guid? AccountCostId{ get; set; } 
        public virtual LedgerAccountDto AccountCostList { get; set; } 
        
        [ForeignKey("AccountSalesLis")]
        public Guid? AccountSalesId{ get; set; }
        public virtual LedgerAccountDto AccountSalesLis { get; set; }
        
        
        [ForeignKey("AccountDiscountList")]
        public Guid? AccountDiscountId{ get; set; }
        public virtual LedgerAccountDto AccountDiscountList { get; set; }
        
        
        [ForeignKey("AccountReturnedList")]
        public Guid? AccountReturnedId{ get; set; }
        public virtual LedgerAccountDto AccountReturnedList { get; set; }
        
        [ForeignKey("AccountCommissionList")]
        public Guid? AccountCommissionId{ get; set; }
        public virtual LedgerAccountDto AccountCommissionList { get; set; } 

        public decimal PricePurchase { get; set; }
        public decimal PriceSale { get; set; }
        public string Commentary { get; set; }
        public string Reference { get; set; }
        public bool IsPurchase { get; set; }
        public bool ForSale { get; set; }

    }

    public class ConceptIdDto : AuditDto
    {
        public string Description { get; set; }
        
        public decimal PricePurchase { get; set; }
        public decimal PriceSale { get; set; } 
        public string Reference { get; set; }
        public bool IsPurchase { get; set; }
        public bool ForSale { get; set; }
        
        [ForeignKey("AccountInventoryList")]
        public Guid? AccountInventoryId{ get; set; }
        public virtual LedgerAccountDto AccountInventoryList { get; set; }
        
        [ForeignKey("AccountCostList")]
        public Guid? AccountCostId{ get; set; } 
        public virtual LedgerAccountDto AccountCostList { get; set; } 
        
        [ForeignKey("AccountSalesLis")]
        public Guid? AccountSalesId{ get; set; }
        public virtual LedgerAccountDto AccountSalesLis { get; set; }
        
        
        [ForeignKey("AccountDiscountList")]
        public Guid? AccountDiscountId{ get; set; }
        public virtual LedgerAccountDto AccountDiscountList { get; set; }
        
        
        [ForeignKey("AccountReturnedList")]
        public Guid? AccountReturnedId{ get; set; }
        public virtual LedgerAccountDto AccountReturnedList { get; set; }
        
        [ForeignKey("AccountCommissionList")]
        public Guid? AccountCommissionId{ get; set; }
        public virtual LedgerAccountDto AccountCommissionList { get; set; } 
    }

}