using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
   public  class QuickLinks : Audit
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Links { get; set; }



    }
}
