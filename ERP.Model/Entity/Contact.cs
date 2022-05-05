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
        public int? IdentificationType { get; set; }
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; } 
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public bool IsClient { get; set; }
        public bool IsSupplier { get; set; }
        public int? ProvinceId { get; set; }
        public int? IsEmployee { get; set; }
        public decimal? Salary { get; set; }
    }
}
