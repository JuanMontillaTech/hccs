using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class ResponseQueryRncRegistered
    {
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        public string RncOCedula { get; set; }
        public string Actividad { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
