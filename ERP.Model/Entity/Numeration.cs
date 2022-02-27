using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Entity
{
    public class Numeration : Audit
    {
        public int DocumentTypeId { get; set; }
        public string Name { get; set; }
        public bool Automatic { get; set; }
        public string Prefix { get; set; }
        public int Sequence { get; set; }
        public int End_Number { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }
        public bool Preferred { get; set; }
        public string Branch { get; set; }
        public string Pie_Invoice { get; set; }
        public int Group { get; set; }  
    }
}
