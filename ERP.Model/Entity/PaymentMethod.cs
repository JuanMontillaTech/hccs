using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Entity
{
    public class PaymentMethod : Audit
    { 
        public string Name { get; set; }
        public bool AdditionalField { get; set; } = false;

        [ForeignKey("Banks")] 
        public Guid? BankId { get; set; }

        public virtual Banks Banks { get; set; }
    }
}
