using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class HeadSemestersDto : AuditDto
    {
        public string InstitutionName { get; set; } // Nombre de la Institución
        public string Code { get; set; } // Código
        public int? NumberOfSisters { get; set; } // Número de Hermanas
        public string Country { get; set; } // País
        public string City { get; set; } // Ciudad
        public int? NumberOfEmployees { get; set; } // Número de Empleados
        public string Address { get; set; } // Dirección
        public string Phone { get; set; } // Teléfono
        public string Fax { get; set; } // Fax
        public int? Year { get; set; } // Año
    }
}
