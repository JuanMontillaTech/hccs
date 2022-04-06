using System;
using ERP.Domain.Command;

namespace ERP.Model.Dtos
{
    public class ConceptDto
    {
        public string Description { get; set; }
        public Guid? CreditLedgerAccountId { get; set; }
        public Guid? DebitLedgerAccountId { get; set; }

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
        public Guid? CreditLedgerAccountId { get; set; }
        public Guid? DebitLedgerAccountId { get; set; }
        public decimal PricePurchase { get; set; }
        public decimal PriceSale { get; set; } 
        public string Reference { get; set; }
        public bool IsPurchase { get; set; }
        public bool ForSale { get; set; }
    }

}