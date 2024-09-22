using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class PaymentMethodDto : AuditDto
    {
        public string Name { get; set; }

        public bool AdditionalField { get; set; } = false;

        [ForeignKey("Banks")] 
        public Guid? BankId { get; set; }

        public virtual BankDto Banks { get; set; }
    }
}