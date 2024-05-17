using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class ResponseQueryNCFDTO
    {
        public string IdentificationIdOrRNC { get; set; }
        public string NameorSocialReason { get; set; }
        public string TypeOfiscalDocument { get; set; }
        public string NCF { get; set; }
        public string Status { get; set; }
        public string ValidUntil { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
