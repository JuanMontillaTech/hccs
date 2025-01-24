using ERP.Domain.Command;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class ContactDto : AuditDto
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
        public bool IsEmployee { get; set; }
        public bool IsSister { get; set; }
        public decimal? Salary { get; set; }
        [ForeignKey("Numeration")]
        public Guid? NumerationId { get; set; }
            [ForeignKey("GroupTaxesTaxes")]
        public Guid? TaxesId { get; set; }  
        public NumerationDto Numeration { get; set; } 
        public Guid? ReferenceId { get; set; }
        


    }
    
}
