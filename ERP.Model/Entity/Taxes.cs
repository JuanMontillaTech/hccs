using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Entity
{
    public class Taxes : Audit
    {
        public string Name { get; set; }

        public decimal Percentage { get; set; }
    }
}
