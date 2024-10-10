using ERP.Domain.Command;
using ERP.Domain.Entity;
using System; 

namespace ERP.Domain.Dtos
{
    public class AssetDto : AuditDto
    {
    
        public string Name { get; set; }
        public Guid AssetClassId { get; set; }
        public Guid LocationId { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public decimal AcquisitionCost { get; set; }
        public decimal? ResidualValue { get; set; }
        public int? UsefulLife { get; set; }
        public string DepreciationMethod { get; set; }
        public string Status { get; set; }
        public string SerialNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        

        // Propiedades de navegación
        public virtual AssetClassDto AssetClass { get; set; }
        public virtual LocationDto Location { get; set; }
    }
}
