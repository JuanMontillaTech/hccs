using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Entity
{

    public class ConfigurationReport : Audit
    {
        public string Title { get; set; }
        public string Details { get; set; }

        [ForeignKey("LedgerAccount")]
        public Guid? Parameter { get; set; }
        public string Criterion { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
        public int Index { get; set; } = 0;

        public virtual LedgerAccount LedgerAccount { get; set; }


    }
}
