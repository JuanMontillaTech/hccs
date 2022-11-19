using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class FileManagerDto
    {
        public string DisplayName { get; set; }

        public string PhysicalName { get; set; }

        public string ContentType { get; set; }

        public string FilePath { get; set; }

        public Guid? SourceId { get; set; }

        public string Folder { get; set; }

        public string SourceTypeName { get; set; }

        public byte[] FormFile { get; set; }

        public bool IsImage
        {
            get
            {
                string path = ContentType;
                if (string.IsNullOrEmpty(path))
                    return false;

                if (path.Equals("image/jpeg") || path.Equals("image/png"))
                {
                    return true;
                }

                return false;
            }
        }

        public Guid Id { get; set; }

        public string CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; }

        public string Commentary { get; set; }
    }
}
