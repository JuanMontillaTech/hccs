using System;
namespace ERP.Domain.Entity
{
    public class ConfigurationPurchase : Audit
    {
        public Guid? AccountPay { get; set; }
        public Guid? AccountPurchase { get; set; }
        public Guid? AccountPurchaseHolding { get; set; }
        public Guid? AccountTaxholding { get; set; }
        public Guid? RefundAccount { get; set; }
        public Guid? AccountAdvance{ get; set; }
        public Guid? Commission { get; set; }
        public Guid? CommissionExpense { get; set; }
    }
}



 
