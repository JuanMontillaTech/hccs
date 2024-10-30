using System;

using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class AssetTransaction : Audit
    {

        public DateTime TransactionDate { get; set; }

        [ForeignKey("Asset")]
        public Guid AssetId { get; set; }

        [ForeignKey("AssetTransactionType")]
        public Guid AssetTransactionTypeId { get; set; }

        [ForeignKey("PaymentMethod")]
        public Guid PaymentMethodId { get; set; }
        public decimal CashAmount { get; set; }
        public decimal CheckNumber { get; set; }
        public decimal Wiretransfer { get; set; }

        public virtual AssetTransactionType AssetTransactionType { get; set; }
        public virtual Asset Asset { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }


    }
}
