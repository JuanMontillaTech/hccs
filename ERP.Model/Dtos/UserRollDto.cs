using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class UserRollDto : AuditDto
    {

        public string Email { get; set; } 

        public Guid RollId { get; set; }

         
    }
    public class UserRollDetallisDto : AuditDto
    {

        public string Email { get; set; }

        [ForeignKey("Rolles")]

        public Guid RollId { get; set; }

        public virtual RollDto Rolles { get; set; }
    }
}

