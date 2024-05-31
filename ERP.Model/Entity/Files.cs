using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ERP.Domain.Entity
{
    public  class Files : Audit
    {
        [DisplayName("Nombre de archivo")]
        public string DisplayName { get; set; }

        public string PhysicalName { get; set; }

        public string ContentType { get; set; }

        public string Folder { get; set; }

        public Guid? SourceId { get; set; }
        public string Commentary { get; set; } 


    }
}
