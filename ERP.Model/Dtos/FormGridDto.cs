using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
   
    public class FormGridDto : AuditDto
    {
        public string DefaultValue { get; set; }
        public bool? IsRequired { get; set; }
        public Guid FormId { get; set; }
        public Guid? SectionId { get; set; }
        public string Field { get; set; }

        public string Label { get; set; }

        public string Index { get; set; }

        public string ColumnIndex { get; set; }

        public string SourceLabel { get; set; }
        public string SourceApi { get; set; }
        public bool ShowForm { get; set; }
        public bool ShowList { get; set; }

        public int Type { get; set; }
    }
    public class FormGridDtoDetallisDto : AuditDto
    {
        [ForeignKey("Froms")]
        public Guid FormId { get; set; }
        public string SourceLabel { get; set; }
        public string Field { get; set; }
        public Guid? SectionId { get; set; }
        public string Label { get; set; }

        public string Index { get; set; }

        public string ColumnIndex { get; set; }

        public string SourceApi { get; set; }
        public string DefaultValue { get; set; }
        public bool? IsRequired { get; set; }

        public bool ShowList { get; set; }
        public bool ShowForm { get; set; }

        public int Type { get; set; }
        public virtual FormDto Froms { get; set; }
    }
}
