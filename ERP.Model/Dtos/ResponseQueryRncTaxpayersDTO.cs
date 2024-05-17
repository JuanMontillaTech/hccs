using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class ResponseQueryRncTaxpayersDTO
    {
        public string IdentificationIdOrRNC { get; set; }
        public string NameorSocialReason { get; set; }
        public string CommercialName { get; set; }
        public string Category { get; set; }
        public string PaymentRegime { get; set; }
        public string Status { get; set; }
        public string EconomicActivity { get; set; }
        public string LocalManagement { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
