using System;
using System.Collections.Generic;

namespace ERP.Domain.Entity
{
    public class Location : Audit
    {
    
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; } 

     
    }
}