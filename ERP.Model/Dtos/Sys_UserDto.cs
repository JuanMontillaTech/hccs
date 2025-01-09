using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class Sys_UserDto : AuditDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DataBaseName { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
