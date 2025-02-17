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
        public bool IsClient { get; set; } = true;
        public bool IsSupplier { get; set; } = true;
        public int? ProvinceId { get; set; }
        public bool IsEmployee { get; set; } = false;
        public bool IsSister { get; set; } = false;
        public decimal? Salary { get; set; } 
        [ForeignKey("Numeration")]
        public Guid? NumerationId { get; set; }
        
        [ForeignKey("GroupTaxesTaxes")]
        public Guid? TaxesId { get; set; }
        public virtual Numeration  Numeration { get; set; }
        
        public  virtual  GroupTaxesTaxes GroupTaxesTaxes { get; set; }
    }
}
