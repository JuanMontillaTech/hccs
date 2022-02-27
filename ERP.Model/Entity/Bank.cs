using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Entity
{
    public class Bank : Audit
    {
        public Guid TypeBankId { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; } 
        public decimal InitialBalance { get; set; }
        public DateTime InicitalDate { get; set; }
        public string Commentary  { get; set; }
        public virtual TypeBank  TypeBank { get; set; }
    }
}
