using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Constants
{
    public interface IBaseEntity
    {
        public Guid Id { get; set; }
        public string LastModifyBy { get; set; }
        public string CreateBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
    }
}
