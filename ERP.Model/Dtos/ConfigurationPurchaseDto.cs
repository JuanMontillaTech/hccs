
using System;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class ConfigurationPurchaseDto : AuditDto
    {

        public Guid? AccountPay { get; set; }
    public Guid? AccountPurchase { get; set; }
    public Guid? AccountPurchaseHolding { get; set; }
    public Guid? AccountTaxholding { get; set; }
    public Guid? RefundAccount { get; set; }
    public Guid? AccountAdvance { get; set; }
    public Guid? Commission { get; set; }
    public Guid? CommissionExpense { get; set; }

}
}

