using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class RequestResponseDto : AuditDto
    {
        public string Number { get; set; }

        public DateTime Date { get; set; }

        public string Response { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid InformationRequestId { get; set; }
    }
}
