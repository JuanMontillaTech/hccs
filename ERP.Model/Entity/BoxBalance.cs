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
        [DisplayName("Caja Disponible")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [DisplayName("Mes")]
        [DisplayFormat( DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime MonthBalance { get; set; } 

    }
}
