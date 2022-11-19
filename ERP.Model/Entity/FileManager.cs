using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
    public partial class FileManager : Audit
    {
        public string DisplayName { get; set; }

        public string PhysicalName { get; set; }

        public string ContentType { get; set; } 

        public Guid? SourceId { get; set; }

        public string Folder { get; set; }     

       
    }
}
