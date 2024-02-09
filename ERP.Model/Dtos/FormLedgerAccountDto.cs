
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class FormLedgerAccountDto
    {
        public Guid IdLedgerAccount { get; set; }
        public Guid IdForm { get; set; }
        public FormDto Form { get; set; }
        public LedgerAccountDto LedgerAccount { get; set; }
    }
}
