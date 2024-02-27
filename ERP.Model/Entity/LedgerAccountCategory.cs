using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class LedgerAccountCategory : Audit
    {
        public string Name { get; set; }

    }
}
