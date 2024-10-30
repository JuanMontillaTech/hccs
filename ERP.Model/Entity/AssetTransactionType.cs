 

namespace ERP.Domain.Entity
{
    public class AssetTransactionType : Audit
    {
        public string Name { get; set; }
        public int? Origen { get; set; } 
    }
}
