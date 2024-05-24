using ERP.Domain.Command; 

namespace ERP.Domain.Dtos
{
    public class SysConfigDto : AuditDto
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
    }
}
