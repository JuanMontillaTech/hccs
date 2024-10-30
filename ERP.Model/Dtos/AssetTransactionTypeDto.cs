using ERP.Domain.Command; 
namespace ERP.Domain.Dtos
{
    public class AssetTransactionTypeDto   : AuditDto
    {
        public string Name { get; set; }
        public int origen { get; set; } = 1;
    }
}
