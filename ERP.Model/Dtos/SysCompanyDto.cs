using ERP.Domain.Command;

namespace ERP.Domain.Dtos;

public class SysCompanyDto : AuditDto
{
    public string CompanyName { get; set; }
    public string DataBaseName { get; set; }
    
}