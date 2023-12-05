using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos;

public class SysUserCompanyDto  : AuditDto
{
    [ForeignKey("SysUser")]
    public Guid? SysUserId { get; set; }
    public virtual Sys_UserDto SysUser { get; set; }
    
    [ForeignKey("SysCompany")]
    public Guid? SysCompanyId { get; set; } 
    
    public virtual SysCompanyDto SysCompany { get; set; }
}


 