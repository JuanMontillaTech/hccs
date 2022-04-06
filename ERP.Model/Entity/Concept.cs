using System;

namespace ERP.Domain.Entity
{
    public class Concept : Audit
    {
        public string Description { get; set; }
        public Guid? CreditLedgerAccountId { get; set; }
        public Guid? DebitLedgerAccountId { get; set; }
        public decimal PricePurchase { get; set; }
        public decimal PriceSale { get; set; }
        public string Reference { get; set; }
        public bool IsPurchase { get; set; }
        public bool ForSale { get; set; }

    }
}
