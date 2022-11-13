using ERP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
   
    public class ManufactureDto : Audit
    {
        public Guid TransactionId { get; set; }

        [ForeignKey("ManufactureStatus")]
        public Guid ManufactureStatusId { get; set; }
        public virtual ManufactureStatusDto ManufactureStatus { get; set; }

    }
    public class ManufactureStatusDto : Audit
    {
        public string Name { get; set; }
        public int Time { get; set; }


    }
}
