using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Entity
{
   public class LedgerAccount : Audit
    {
        public Guid? Belongs { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Commentary { get; set; }
        public int Nature { get; set; } 
        public int LocationStatusResult { get; set; }
    }
}
