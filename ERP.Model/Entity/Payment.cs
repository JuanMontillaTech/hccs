using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Entity
{
    public class Payment : Audit
    {
        public Guid EmployeeId { get; set; }
        public DateTime PaymentPeriodStart { get; set; }
        public DateTime PaymentPeriodEnd { get; set; }
        public string Code { get; set; }
        public int TypePay { get; set; }
        public string Concept { get; set; }
        public int Count { get; set; }
        public decimal Value { get; set; }

    }
}
