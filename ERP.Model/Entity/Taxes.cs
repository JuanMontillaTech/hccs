using System; 
namespace ERP.Domain.Entity
{
    public class Taxes : Audit
    {
       
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public int TaxType { get; set; }
        public Guid? CreditLedgerAccountId { get; set; }
        public Guid? DebitLedgerAccountId { get; set; }
    }
}
