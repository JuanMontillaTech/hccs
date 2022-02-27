using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Entity
{
    public class Contact : Audit
    { 
        public string Name { get; set; }
        public string Identity { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

    }
}
