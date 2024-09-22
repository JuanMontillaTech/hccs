using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class DataWay : Audit
    { 
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
