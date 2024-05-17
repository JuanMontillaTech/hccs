using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class ResponseQueryRncRegisteredDTO
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string IdentificationIdOrRNC { get; set; }
        public string Activity { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
