using ERP.Domain.Command;
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
        public string Flow { get; set; }
        public string Controller { get; set; }
        public int Index { get; set; }
        public bool? Edit { get; set; }
        public bool? Create { get; set; } 
        public bool? Delete { get; set; }
        public bool? BackList { get; set; }

        
        
    }
}
