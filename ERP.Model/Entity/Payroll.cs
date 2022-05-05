using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Entity
{
    public class Payroll : Audit
    {
        public Guid ContactId { get; set; }
        public int TypePay { get; set; }
        public string Concept { get; set; } 
        public int Count { get; set; }
        public decimal Value { get; set; }

    }
}
