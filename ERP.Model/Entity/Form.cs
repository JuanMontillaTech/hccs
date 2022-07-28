using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Domain.Entity
{
    public class Form : Audit
    {
        public string Title { get; set; }

        public string Label { get; set; }

        public Guid? ModuleId { get; set; }

        public int TransactionsType { get; set; }

        public string FormCode { get; set; }

        public string Controller { get; set; }

        //public bool IsClient { get; set; }

        //public string DateLabel { get; set; }

        public string Path { get; set; }

        //public int DocumentTypeId { get; set; }

        public string Flow { get; set; }

        public int Index { get; set; }

       
    }
}
