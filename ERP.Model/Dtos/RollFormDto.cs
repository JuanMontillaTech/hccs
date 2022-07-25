using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Dtos
{
    public class RollFormDto
    {
      
        public Guid? RollId { get; set; }

         
        public Guid? FormId { get; set; }
  
    }
    public class RollFormDetallisDto
    {
        [ForeignKey("Rolles")]
        public Guid? RollId { get; set; }


        [ForeignKey("Froms")]
        public Guid? FormId { get; set; }

        public virtual RollDto Rolles { get; set; }
        public virtual FormDto Froms { get; set; }
    }
}

