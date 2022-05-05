using ERP.Domain.Command;

using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class PayrollDto : AuditDto
    {
        public Guid ContactId { get; set; }
        public int TypePay { get; set; }
        public string Concept { get; set; }
        public int Count { get; set; }
        public decimal Value { get; set; }
    }
}
