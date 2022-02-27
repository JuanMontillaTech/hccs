using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Entity
{
    public class TypeRegister : Audit
    {
        public string Name { get; set; }

        public virtual ICollection<Journal>  Journals { get; set; }
    }
}
