using ERP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class TransactionLocation : Audit
    {
       
        public string Name { get; set; }

        public int Index { get; set; } = 0; 
    }
}
