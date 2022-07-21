using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Entity
{
    public class Currency : Audit
    {
        public string Code { get; set; }        

        public string Name { get; set; } 

        public string Country { get; set; }
    }
}
