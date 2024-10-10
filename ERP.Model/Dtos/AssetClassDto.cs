using ERP.Domain.Command; 

namespace ERP.Domain.Dtos
{
    public class AssetClassDto : AuditDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AccountCode { get; set; }
    }
 
}
