using ERP.Domain.Command;
using ERP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
   
    public class PrintingTemplateDto : AuditDto
    {
        public string Name { get; set; }         

        public string Template { get; set; }
    }
}
