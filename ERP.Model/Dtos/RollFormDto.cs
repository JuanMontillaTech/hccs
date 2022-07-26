using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class RollFormDto : AuditDto
    {
      
        public Guid? RollId { get; set; }

         
        public Guid? FormId { get; set; }
  
    }
    public class RollFormDetallisDto : AuditDto
    {
        [ForeignKey("Rolles")]
        public Guid? RollId { get; set; }


        [ForeignKey("Froms")]
        public Guid? FormId { get; set; }

        public virtual RollDto Rolles { get; set; }
        public virtual FormDto Froms { get; set; }
    }
}

