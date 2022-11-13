using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.AbstractEntiy
{
    public class AbstractConcept : AuditDto
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
        public bool? IsPurchase { get; set; } = false;
        public bool? ForSale { get; set; } = true;
        public bool? SellOutStock { get; set; } = true;
        public bool? IsServicie { get; set; } = false;
        public decimal? Stock { get; set; } = 0;
    }
}
