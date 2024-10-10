using System;

namespace ERP.Domain.Entity
{
    public class Asset : Audit
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
        public virtual AssetClass AssetClass { get; set; }
        public virtual Location Location { get; set; }
    }
}