using ERP.Domain.Command;

using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class TransactionsDto : AuditDto
    {
        public Guid? ContactId { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public Guid PaymentMethodId { get; set; }
        public decimal GlobalDiscount { get; set; }
        public decimal GlobalTotal { get; set; }
        public int? TransactionsType { get; set; }
        public virtual List<TransactionsDetailsDto> TransactionsDetails { get; set; }
    }

    public class TransactionsDetailsDto : AuditDto
    {
        public Guid? TransactionsId { get; set; }
        public Guid? ReferenceId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
    }
}
