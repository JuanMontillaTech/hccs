using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Model.Entity
{
    public class Transaction : Audit
    { 
        public string Name { get; set; }
    }
}
