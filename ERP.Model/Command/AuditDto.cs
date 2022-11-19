using System;
using System.Collections.Generic;
using System.Text;
 
using ERP.Domain.Dtos;

namespace ERP.Domain.Command
{
    public abstract class AuditDto
    {
        public Guid? Id { get; set; }
        public string LastModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string Commentary { get; set; }
       
    }
}
