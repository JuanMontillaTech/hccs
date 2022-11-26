using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class ConceptElement : Audit
    {
        [ForeignKey("Concepts")]
        public Guid ConceptId { get; set; }
        public string Name { get; set; }        
        public virtual Concept Concepts { get; set; }

    }
}
