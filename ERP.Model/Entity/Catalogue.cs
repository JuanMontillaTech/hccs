using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class Catalogue  : Audit
    {
        public string Name { get; set; }
        
        public bool Enable { get; set; }

        public bool Service { get; set; }
        
        public bool ShowPos { get; set; }

        [ForeignKey("AccountInventoryList")]
        public Guid? AccountInventory{ get; set; }
        public virtual LedgerAccount AccountInventoryList { get; set; }
        
        [ForeignKey("AccountCostList")]
        public Guid? AccountCost{ get; set; } 
        public virtual LedgerAccount AccountCostList { get; set; } 
        
        [ForeignKey("AccountSalesLis")]
        public Guid? AccountSales{ get; set; }
        public virtual LedgerAccount AccountSalesLis { get; set; }
        
        
        [ForeignKey("AccountDiscountList")]
        public Guid? AccountDiscount{ get; set; }
        public virtual LedgerAccount AccountDiscountList { get; set; }
        
        
        [ForeignKey("AccountReturnedList")]
        public Guid? AccountReturned{ get; set; }
        public virtual LedgerAccount AccountReturnedList { get; set; }
        
        [ForeignKey("AccountCommissionList")]
        public Guid? AccountCommission{ get; set; }
        public virtual LedgerAccount AccountCommissionList { get; set; } 
    }
}