using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class FormfieldsDto : AuditDto
    {
        public string DefaultValue { get; set; }
        public bool? IsRequired { get; set; }
        public Guid FormId { get; set; }
        public Guid? SectionId { get; set; }
        public string Field { get; set; }
        public Guid? FormSoportId { get; set; }
        public bool ReadOnly { get; set; } = false;
        public string Label { get; set; }

        public string Index { get; set; }

        public string ColumnIndex { get; set; }

        public string SourceLabel { get; set; }
        public string SourceApi { get; set; }
        public bool ShowForm { get; set; }
        public bool ShowList { get; set; }
        public bool? ShowSub { get; set; } = false;
        public int Type { get; set; }

        public string Css { get; set; }
    }

    public class FormfieldsDetallisDto : AuditDto
    {
        [ForeignKey("Froms")] public Guid FormId { get; set; }
        public string SourceLabel { get; set; }
        public string Field { get; set; }
        public Guid? SectionId { get; set; }
        public string Label { get; set; }
        public Guid? FormSoportId { get; set; }
        public bool? ShowSub { get; set; } = false;
        public string Index { get; set; }
        public bool ReadOnly { get; set; } = false;
        public string ColumnIndex { get; set; }

        public string SourceApi { get; set; }
        public string DefaultValue { get; set; }
        public bool? IsRequired { get; set; }

        public bool ShowList { get; set; }
        public bool ShowForm { get; set; }
        public string Css { get; set; }
        public int Type { get; set; }
        public virtual FormDto Froms { get; set; }
    }
}