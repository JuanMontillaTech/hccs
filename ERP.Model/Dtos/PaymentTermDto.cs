using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class PaymentTermDto : AuditDto
    {
        public string Name { get; set; }

    }
}