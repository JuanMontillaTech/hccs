using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class BoxCloseDto : AuditDto
    {
         
        public string Code { get; set; }
        
        public decimal GlobalTotal { get; set; }
         

    }
}
