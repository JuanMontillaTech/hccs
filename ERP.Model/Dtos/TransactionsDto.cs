using ERP.Domain.Command;
using ERP.Domain.Entity;
using ERP.Model.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class TransactionsDto : AuditDto
    {
        public Guid  FormId { get; set; }
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

        public decimal GlobalDiscount { get; set; }
        public decimal GlobalTotal { get; set; } 
        public decimal TotalAmount { get; set; } = 0;
        public int TransactionsType { get; set; }
        
        public virtual List<TransactionsDetailsDto> TransactionsDetails { get; set; }
        public virtual TransactionStatusDto TransactionStatus { get; set; }
        public virtual PaymentMethodDto PaymentMethods { get; set; }
      
        public virtual PaymentTermDto  PaymentTerms { get; set; }
        public virtual Contact Contact { get; set; }
    } 
    public class TransactionsContactDto : AuditDto
    {
        //Contacto 
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; } 
        public string CellPhone { get; set; }
        public string Phone1 { get; set; }
        //Cotización
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public Guid? ContactId { get; set; }
        public Guid? NumerationId { get; set; }

        public decimal TotalAmount { get; set; } = 0;
        public Guid? CurrencyId { get; set; }

        //Facturación  a credito
        [ForeignKey("PaymentTerms")]
      

        public Guid? PaymentTermId { get; set; }

        //Facturación  al contado
        [ForeignKey("PaymentMethods")]
        public Guid? PaymentMethodId { get; set; }

        public Guid? FormId { get; set; }


        public Guid? TaxNumber { get; set; }

        public Guid? TransactionStatusId { get; set; }
        public decimal GlobalDiscount { get; set; }
        public decimal GlobalTotal { get; set; }
        public int TransactionsType { get; set; }
        public virtual List<TransactionsDetailsDto> TransactionsDetails { get; set; }
        public virtual TransactionStatusDto TransactionStatus { get; set; }
        public virtual PaymentMethodDto PaymentMethods { get; set; }
      
        public virtual PaymentTermDto  PaymentTerms { get; set; }
    }

    public class TransactionsDetailsDto : AuditDto
    {
        public Guid? TransactionsId { get; set; }
        [ForeignKey("Concept")]
        public Guid? ReferenceId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public virtual ConceptDto Concept { get; set; }
        public virtual List<TransactionsDetailsElementDto> TransactionsDetailsElement { get; set; }
    }
}
