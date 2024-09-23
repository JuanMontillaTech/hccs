using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class CsvData
    {
        public string TIPO { get; set; }
        public string COD { get; set; }
        public string CUENTA { get; set; }
        public decimal ENERO { get; set; }
        public decimal FEBRERO { get; set; }
        public decimal MARZO { get; set; }
        public decimal ABRIL { get; set; }
        public decimal MAYO { get; set; }
        public decimal JUNIO { get; set; }
    
        public decimal JULIO { get; set; }
        public decimal AGOSTO { get; set; }
        public decimal SEPTIEMBRE { get; set; }
        public decimal OCTUBRE { get; set; }
        public decimal NOVIEMBRE { get; set; }
        public decimal DICIEMBRE { get; set; }

    }
}
