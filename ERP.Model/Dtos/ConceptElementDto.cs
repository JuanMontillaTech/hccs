using ERP.Domain.Command;
using ERP.Domain.Entity;
using ERP.Model.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    
    public class ConceptElementDto : AuditDto
    {
        [ForeignKey("Concepts")]
        public Guid ConceptId { get; set; }
        public string Name { get; set; }
        public virtual ConceptDto Concepts { get; set; }

    }
}
