using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.Domain.Entity
{
    public abstract class Audit
    {
        [Key]

        public Guid Id { get; set; }
        [DisplayName("Modificacdo Por")]
        public string LastModifiedBy { get; set; }
        [DisplayName("Creado")]
        public string CreatedBy { get; set; }
        [DisplayName("Fecha Modificación")]
        public DateTime LastModifiedDate { get; set; }
        [DisplayName("Fecha de Creación")]
        public DateTime CreatedDate { get; set; }
        public string Commentary { get; set; } 
    
        [DisplayName("Estado")]
        public bool IsActive { get; set; }

    }
}
