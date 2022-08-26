using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System;
using System.Collections.Generic;
namespace ERP.Domain.Entity
{
    public abstract class Audit
    {
        [Key]

        public Guid Id { get; set; } 
        public string LastModifiedBy { get; set; }         
        public string CreatedBy { get; set; }      
        public DateTime LastModifiedDate { get; set; }       
        public DateTime CreatedDate { get; set; }
        public string Commentary { get; set; }             
        public bool IsActive { get; set; }

    }
}
