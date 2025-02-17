﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class Formfields : Audit
    {
        [ForeignKey("Froms")]
        public Guid FormId { get; set; }
        public Guid? FormSoportId { get; set; }
        
        [ForeignKey("Section")]
        public Guid? SectionId { get; set; }
        
        public string Field { get; set; }
        
        public bool ReadOnly { get; set; } = false;

        public string Label { get; set; }

        public string Index { get; set; }
        public bool ShowForm { get; set; }
        public string ColumnIndex { get; set; }
        public string SourceApi { get; set; }
        public string SourceLabel { get; set; }
        public string DefaultValue { get; set; }
        public bool? IsRequired { get; set; }

        public bool ShowList { get; set; }
        public bool? ShowSub { get; set; } = false;
        public int Type { get; set; }
     
        public string Css { get; set; }
        public virtual Form Froms { get; set; }
        public virtual Section  Section { get; set; }
    }
}

