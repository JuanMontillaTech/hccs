using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class AssetClass : Audit
    {
    
        public string Name { get; set; }
        public string Description { get; set; }
        public string AccountCode { get; set; }
   
    }
}
