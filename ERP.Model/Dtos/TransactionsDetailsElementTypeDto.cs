using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class TransactionsDetailsElementTypeDto : AuditDto
    {
        public string Name { get; set; }
        public int? Index { get; set; } = 0;
    }
}
