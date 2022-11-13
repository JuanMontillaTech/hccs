using ERP.Domain.AbstractEntiy;
 
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class Concept : AbstractConcept 
    {
         
        public virtual Catalogue Catalogues { get; set; } 
        public virtual LedgerAccount AccountInventoryList { get; set; }
        public virtual LedgerAccount AccountCostList { get; set; }
        public virtual LedgerAccount AccountSalesLis { get; set; }
        public virtual LedgerAccount AccountDiscountList { get; set; }
        public virtual LedgerAccount AccountReturnedList { get; set; }
        public virtual LedgerAccount AccountCommissionList { get; set; }
    }
}
