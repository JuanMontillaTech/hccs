using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class DataWayDto : AuditDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
