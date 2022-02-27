using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Model.Entity
{
    public class Contact : Audit
    {
       
        public string Reference { get; set; }
        public string Commentary { get; set; } 
        public DateTime Date { get; set; }
 
        public virtual ICollection<ContactDetails> ContactDetails { get; set; }


    }
    public class ContactDetails : Audit
    {
        public int Contact { get; set; }
        public Guid ContactId { get; set; } 
        public string Commentary { get; set; }
        public virtual Contact Contact { get; set; }

    }
}
