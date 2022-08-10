using System;
using System.Collections.Generic;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class ModuleDto : AuditDto
    {
        public string Label { get; set; }

        public string Icon { get; set; }

        public string Link { get; set; }

        public bool IsTitle { get; set; }

        public int Index { get; set; } = 0;

        public virtual List<FormDto> Froms { get; set; }
    }
}

