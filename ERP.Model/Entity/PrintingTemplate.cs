using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class PrintingTemplate : Audit
    {
        public string Name { get; set; }
         

        public string  Template { get; set; }
    }
}
