using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class FileLinkDto 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string link { get; set; }
        public Guid SourceId { get; set; }

    }
}
