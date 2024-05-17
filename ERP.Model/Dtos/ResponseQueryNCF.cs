using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class ResponseQueryNCF
    {
        public string RncOCedula { get; set; }
        public string NombreORazónSocial { get; set; }
        public string TipoDeComprobante { get; set; }
        public string NCF { get; set; }
        public string Estado { get; set; }
        public string VálidoHasta { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
 