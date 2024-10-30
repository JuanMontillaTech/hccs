using ERP.Domain.Command;
using ERP.Domain.Entity;
using System; 
using System.ComponentModel.DataAnnotations.Schema;  

namespace ERP.Domain.Dtos
{
    public class AssetTransactionDto : AuditDto
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

        public virtual AssetTransactionTypeDto AssetTransactionType { get; set; }
        public virtual AssetDto Asset { get; set; }
        public virtual PaymentMethodDto PaymentMethod { get; set; }


    }
}
