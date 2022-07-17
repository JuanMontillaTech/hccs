using System;
using System.Collections.Generic;

namespace ERP.Domain.Entity
{
    public class Module : Audit
    {
        public string Label { get; set; }

        public string Icon { get; set; }

        public string Link { get; set; }

        public bool IsTitle { get; set; }

        public virtual List<Form> Froms { get; set; }

    }
}

