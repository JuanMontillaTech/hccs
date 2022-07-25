using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Dtos
{
    public class UserRollDto
    {

        public string Email { get; set; } 

        public Guid RollId { get; set; }

         
    }
    public class UserRollDetallisDto
    {

        public string Email { get; set; }

        [ForeignKey("Rolles")]

        public Guid RollId { get; set; }

        public virtual RollDto Rolles { get; set; }
    }
}

