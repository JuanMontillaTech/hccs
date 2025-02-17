﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace ERP.Domain.Entity
{
    public abstract class Audit : ICloneable
    {
        [Key]

        public Guid Id { get; set; } 
        public string LastModifiedBy { get; set; }         
        public string CreatedBy { get; set; }      
        public DateTime LastModifiedDate { get; set; }       
        public DateTime CreatedDate { get; set; }
        public string Commentary { get; set; }             
        public bool IsActive { get; set; }
        [NotMapped]
        public byte[] File { get; set; }

        [NotMapped]
        public List<FileManager> Files { get; set; } = new List<FileManager>();

         public object Clone()
        {
            Audit clon = (Audit)this.MemberwiseClone(); 
            clon.Files = new List<FileManager>(this.Files);  

            return clon;
        }

    }
}
