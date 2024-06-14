using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Entity
{
    public class BoxBalance : Audit
    {
        
        public decimal? Balance { get; set; } = 0;
 
        public DateTime MonthBalance { get; set; } 

    }
}
