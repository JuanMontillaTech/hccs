using ERP.Domain.Command;

using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class TransactionStatusDto : AuditDto
    {
        public string Name { get; set; }
    }
}
