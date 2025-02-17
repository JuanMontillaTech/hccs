﻿using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class FormDto : AuditDto
    {
        public string Title { get; set; }
        public string Label { get; set; }
        public int TransactionsType { get; set; }
        public string FormCode { get; set; }
        public string Path { get; set; }
        public Guid? ModuleId { get; set; }
        public Guid? NumerationsId { get; set; }
        public Guid? FormDetailId { get; set; }
        public string FormSubTitle{ get; set; }
        public string FormDetailName { get; set; }
        public string FormDetailFieldName { get; set; }
        public string Flow { get; set; }
        public string Controller { get; set; }
        public int Index { get; set; }
        public bool? Edit { get; set; }
        public bool? Create { get; set; }
        public bool? Print { get; set; }
        public bool? Delete { get; set; }
        public bool? BackList { get; set; }  
        public bool? Plus { get; set; }
        public bool? Upload { get; set; }
        public bool? Show { get; set; }
     
        public string Prefix { get; set; }

        public int? Sequence { get; set; } = 0;
        
        public bool? AllowSequence { get; set; }

        public int? ModeViewForm { get; set; } = 0;

    }
}
