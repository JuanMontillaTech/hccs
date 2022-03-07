using ERP.Domain.Command;
using ERP.Domain.Entity;
using System;
using System.Collections.Generic;

public class JournalDto
    {
         public string Code { get; set; }
        public string Reference { get; set; }
        public string Commentary { get; set; }
        public Guid TypeRegisterId { get; set; }
        public DateTime Date { get; set; }

        public virtual TypeRegister  TypeRegister  { get; set; }
        public virtual ICollection<JournaDetailsDto> JournaDetails { get; set; }

    }
    public class JournalIdDto : AuditDto
    {
        public Guid? Belongs { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Commentary { get; set; }
        public int Nature { get; set; } 
        public int LocationStatusResult { get; set; }
    }







    public class JournaDetailsDto : AuditDto
    {
        public Guid? Belongs { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Commentary { get; set; }
        public int Nature { get; set; } 
        public int LocationStatusResult { get; set; }
    }

