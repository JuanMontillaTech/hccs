﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class TicketDto
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhones { get; set; }
        public string CompanyAdress { get; set; }
        public Guid InvoiceId { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceComentary { get; set; }
        public decimal InvoiceTotal { get; set; }
        public Guid? InvoicePaymentMethodId { get; set; }
        public string InvoicePaymentMethod { get; set; }
        public Guid? InvoicePaymentTermId { get; set; }
        public string InvoicePaymentTerm { get; set; }
        public Guid? InvoiceContactId { get; set; }
        public string InvoiceContactName { get; set; }
        public string InvoiceContactPhone { get; set; }
        public string InvoiceContactAdress { get; set; }
        public List<TicketDetallisDto> TicketDetallisDtos { get; set; }
    }
    public class TicketDetallisDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid? ReferenceId { get; set; }
        public string Reference { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}