using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class FormDto : AuditDto
    {
        public string Title { get; set; }
        public int TransactionsType { get; set; }
        public string FormCode { get; set; }
        public bool IsClient { get; set; }
        public string DateLabel { get; set; }
        public string Path { get; set; }
        public int DocumentTypeId { get; set; }
    }
}
