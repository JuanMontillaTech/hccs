using System;
namespace ERP.Domain.Entity
{
    public class ConfigurationSell : Audit
    {
        public Guid? Accountcollect{ get; set; }
        public Guid? AccountSelling{ get; set; }
        public Guid? DiscountSales{ get; set; }
        public Guid? AccountDiscountReceived{ get; set; }
        public Guid? AccountAdvance{ get; set; }
        public Guid? AccountCheckReturned{ get; set; }
        public Guid? AccountITBISexpenses{ get; set; }
    }
}

