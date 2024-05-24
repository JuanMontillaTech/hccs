using ERP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class SysStatisticDto
    {
        [ForeignKey("ReportQuery")]
        public Guid ReportQueryId { get; set; }

        public virtual ReportQuery ReportQuery { get; set; }

        [ForeignKey("Form")]
        public Guid FormId { get; set; }

        public virtual FormDto Form { get; set; }

        [ForeignKey("Config")]
        public Guid ConfigId { get; set; }

        public virtual SysConfigDto Config { get; set; }
    }
}
