using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class Entidades
    {
        public int Id { get; set; } // Identificador único autoincrementable
        public string Nombre { get; set; } // Nombre de la institución
        public string Codigo { get; set; } // Código de la institución (opcional)
        public string Pais { get; set; } // País donde se encuentra la institución
        public string Ciudad { get; set; } // Ciudad donde se encuentra la institución
        public string Departamento { get; set; } // Departamento o estado donde se encuentra la institución
        public string Direccion { get; set; } // Dirección de la institución (opcional)
        public string Telefono { get; set; } // Número de teléfono de la institución (opcional)
        public string Fax { get; set; } // Número de fax de la institución (opcional)
        public int NumeroHermanas { get; set; } // Número de instituciones hermanas (opcional)
        public int NumeroEmpleados { get; set; } // Número de empleados de la institución (opcional)
    }
}
