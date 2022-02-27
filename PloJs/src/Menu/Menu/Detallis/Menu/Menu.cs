using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Model.Entity
{
    public class Menu : Audit
    {
       
        public string Reference { get; set; }
        public string Commentary { get; set; } 
        public DateTime Date { get; set; }
 
        public virtual ICollection<MenuDetails> MenuDetails { get; set; }


    }
    public class MenuDetails : Audit
    {
        public int Contact { get; set; }
        public Guid MenuId { get; set; } 
        public string Commentary { get; set; }
        public virtual Menu Menu { get; set; }

    }
}
