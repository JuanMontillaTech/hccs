﻿using ERP.Domain.Constants;

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
 
        public string TaxNumber { get; set; }
    
        [ForeignKey("PaymentTerms")]
        public Guid? PaymentTermId { get; set; }
        public Guid? CurrencyId { get; set; }
        public string TaxContactNumber { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        [ForeignKey("TransactionStatus")]
        public Guid? TransactionStatusId { get; set; }
        
        [ForeignKey("PaymentMethods")]
        public Guid? PaymentMethodId { get; set; }
        
        [ForeignKey("GroupTaxes")]
        public Guid? TaxesGroupId { get; set; }
    [ForeignKey("Box")]
        public Guid? BoxId { get; set; }

        public decimal GlobalDiscount { get; set; }
        public decimal GlobalTotal { get; set; } 
        public decimal GlobalTotalTax { get; set; } 
        public decimal TotalAmount { get; set; } = 0;
        public decimal TotalAmountTax { get; set; } = 0;
        public decimal TotalTax { get; set; } = 0;
        
        
        public int TransactionsType { get; set; }
        public virtual List<TransactionsDetails> TransactionsDetails { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Box Box { get; set; }
        public virtual TransactionStatus TransactionStatus { get; set; }
        public virtual PaymentMethod PaymentMethods { get; set; }
        public virtual PaymentTerm PaymentTerms { get; set; }
        public virtual GroupTaxes  GroupTaxes { get; set; }

        
    }
    public class TransactionsDetails : Audit
    {
        public Guid TransactionsId { get; set; }
        [ForeignKey("Concept")]
        public Guid? ReferenceId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        
        public decimal PriceWithTax { get; set; }
        
        public decimal TotalTax { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }

        public virtual List<TransactionsDetailsElement> TransactionsDetailsElement { get; set; }

        public virtual  Concept Concept { get; set; }


    }
}
