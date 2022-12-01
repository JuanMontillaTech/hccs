using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class TransactionsDetailsElementType :Audit
    {
        public string Name { get; set; }
        public int? Index { get; set; } = 0;
    }
}
