using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.AbstractEntiy;
using ERP.Domain.Command;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;

namespace ERP.Model.Dtos
{
    public class ConceptDto : AbstractConcept
    {
        
        public virtual CatalogueDto  Catalogues { get; set; }
        public virtual LedgerAccountDto AccountInventoryList { get; set; }
        public virtual LedgerAccountDto AccountCostList { get; set; }
        public virtual LedgerAccountDto AccountSalesLis { get; set; }
        public virtual LedgerAccountDto AccountDiscountList { get; set; }
        public virtual LedgerAccountDto AccountReturnedList { get; set; }
        public virtual LedgerAccountDto AccountCommissionList { get; set; }
    }
 

}