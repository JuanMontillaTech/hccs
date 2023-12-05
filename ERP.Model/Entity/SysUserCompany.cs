using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity;

public class SysUserCompany  : Audit
{
    [ForeignKey("SysUser")]
    public Guid? SysUserId { get; set; }
    public virtual SysUser SysUser { get; set; }
    
    [ForeignKey("SysCompany")]
    public Guid? SysCompanyId { get; set; } 
    
    public virtual SysCompany SysCompany { get; set; }
}


 