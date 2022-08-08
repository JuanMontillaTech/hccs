using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
namespace ERP.Domain.Command
{
    public abstract class AuditDto
    {
        public Guid? Id { get; set; }
        public string LastModifiedBy { get; set; } = "prueba@gmail.com";
        public string CreatedBy { get; set; } = "prueba@gmail.com";
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string Commentary { get; set; } = "Commentary";
    }
}
