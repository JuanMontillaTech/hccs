using ERP.Domain.Command;

using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class ContactDto
    {
        public string Name { get; set; }
        public string Identity { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
    }
    public class ContactIdDto : AuditDto
    {
        public string Name { get; set; }
        public string Identity { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
    }
}
