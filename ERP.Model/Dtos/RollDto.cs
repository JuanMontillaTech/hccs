using System;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class RollDto : AuditDto
    {
        public string Name { get; set; }
    }
}

