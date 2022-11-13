using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public class Manufacture : Audit
    {
        public Guid TransactionId { get; set; }

        [ForeignKey("ManufactureStatus")]
        public Guid ManufactureStatusId { get; set; }
        public virtual ManufactureStatus ManufactureStatus { get; set; }

    }
    public class ManufactureStatus : Audit
    {
        public string Name { get; set; }
        public int Time { get; set; }


    }
}
