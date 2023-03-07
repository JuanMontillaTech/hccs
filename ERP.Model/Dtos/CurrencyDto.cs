using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class CurrencyDto : AuditDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }
        
        public  decimal Rate { get; set; }
        
        

    }
}
