using System;
using System.Collections.Generic;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class SectionDto : AuditDto
    {
        public string Name { get; set; }
        
        
    }
    public class SectionFieldsDto : AuditDto
    {
        public string Name { get; set; }
        
        
        public virtual  List<FormfieldsDetallisDto>  Fields { get; set; }
    }
    public class SectionFormGrid : AuditDto
    {
        public string Name { get; set; }

        public virtual List<FormGridDtoDetallisDto> Fields { get; set; }
    }
}