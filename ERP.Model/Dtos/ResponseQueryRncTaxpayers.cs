using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class ResponseQueryRncTaxpayers
    {
        public string CedulaORnc { get; set; }
        public string NombreORazónSocial { get; set; }
        public string NombreComercial { get; set; }
        public string Categoría { get; set; }
        public string RegimenDePagos { get; set; }
        public string Estado { get; set; }
        public string ActividadEconomica { get; set; }
        public string AdministracionLocal { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
