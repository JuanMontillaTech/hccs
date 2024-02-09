using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class FormLedgerAccount : Audit
    {
        public Guid IdLedgerAccount { get; set; }
        public Guid IdForm { get; set; }
        public Form Form { get; set; }  
        public LedgerAccount LedgerAccount { get; set; }
    }
}
