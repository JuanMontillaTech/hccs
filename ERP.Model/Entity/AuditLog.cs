using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class AuditLog
    {
        public string UserId { get; set; }
        public DateTime LoginTimestamp { get; set; } = DateTime.Now;
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Error { get; set; }
    }
}
