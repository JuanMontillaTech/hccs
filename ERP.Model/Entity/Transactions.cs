using ERP.Domain.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Entity
{
    public class Transactions : Audit
    {
        [ForeignKey("Contact")]
        public Guid? ContactId { get; set; }
        public Guid? NumerationId { get; set; }
        public Guid? PaymentTermId { get; set; }
        public Guid? CurrencyId { get; set; }
        public Guid? TaxNumber { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        [ForeignKey("TransactionStatus")]
        public Guid? TransactionStatusId { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public decimal GlobalDiscount { get; set; }
        public decimal GlobalTotal { get; set; }
        public int TransactionsType { get; set; }
        public virtual List<TransactionsDetails> TransactionsDetails { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual TransactionStatus TransactionStatus { get; set; }

    }
    public class TransactionsDetails : Audit
    {
        public Guid TransactionsId { get; set; }
        [ForeignKey("Concept")]
        public Guid? ReferenceId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public virtual  Concept Concept { get; set; }
    }
}
