using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class FormfieldsDto : AuditDto
    {
         
        public Guid FormId { get; set; }
        public string Field { get; set; }

        public string Label { get; set; }

        public string Index { get; set; }

        public string ColumnIndex { get; set; }

        public string SourceLabel { get; set; }
        public string SourceApi { get; set; }

        public bool ShowList { get; set; }

        public int Type { get; set; }
    }
    public class FormfieldsDetallisDto : AuditDto
    {
        [ForeignKey("Froms")]
        public Guid FormId { get; set; }
        public string SourceLabel { get; set; }
        public string Field { get; set; }

        public string Label { get; set; }

        public string Index { get; set; }

        public string ColumnIndex { get; set; }

        public string SourceApi { get; set; }

        public bool ShowList { get; set; }

        public int Type { get; set; }
        public virtual FormDto Froms { get; set; }
    }
}

