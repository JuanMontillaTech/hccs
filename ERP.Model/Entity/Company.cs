﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Entity
{
     
     public class Company : Audit
    {
        public string TaxId { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string Email { get; set; }
        public string Phones { get; set; }
        public string CompanyCode { get; set; }
        public Guid? UserAdmin { get; set; }
        public int Collaborators { get; set; } = 1;
        public int Employees { get; set; } = 1;

    }
}