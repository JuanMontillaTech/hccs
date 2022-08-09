using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Dtos
{
    public class CatalogueDto : AuditDto
    {
        public string Name { get; set; }
        
        public bool Enable { get; set; }

        public bool Service { get; set; }
        
        public bool ShowPos { get; set; }

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