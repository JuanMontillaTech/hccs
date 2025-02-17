﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
   
    public class FormGrid : Audit
    {
        [ForeignKey("Froms")]
        public Guid FormId { get; set; }

        [ForeignKey("Section")]
        public Guid? SectionId { get; set; }

        public string Field { get; set; }

        public string Label { get; set; }

        public string Index { get; set; }
        public bool ShowForm { get; set; }
        public string ColumnIndex { get; set; }
        public string SourceApi { get; set; }
        public string SourceLabel { get; set; }
        public string DefaultValue { get; set; }
        public bool? IsRequired { get; set; }

        public bool ShowList { get; set; }

        public int Type { get; set; }
        public virtual Form Froms { get; set; }
        public virtual Section Section { get; set; }
    }
}
