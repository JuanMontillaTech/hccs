using ERP.Domain.Command;
using System;

public class LedgerAccountDto
    {
        public Guid? Belongs { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Commentary { get; set; }
        public int Nature { get; set; } 
        public int LocationStatusResult { get; set; }

    }
    public class LedgerAccountIdDto : AuditDto
    {
        public Guid? Belongs { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Commentary { get; set; }
        public int Nature { get; set; } 
        public int LocationStatusResult { get; set; }
    }