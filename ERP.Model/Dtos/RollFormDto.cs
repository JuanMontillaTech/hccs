using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;
using ERP.Domain.Entity;

namespace ERP.Domain.Dtos
{
    public class RollFormDto : AuditDto
    {
      
        [ForeignKey("Rolles")]
        public Guid? RollId { get; set; }


        [ForeignKey("Froms")]
        public Guid? FormId { get; set; }

        public virtual Roll Rolles { get; set; }
        public virtual Form Froms { get; set; }
  
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

