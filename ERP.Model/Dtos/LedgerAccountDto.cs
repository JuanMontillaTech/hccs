using ERP.Domain.Command;
using System;
using System.Collections.Generic;

public class LedgerAccountDto : AuditDto
{
        public Guid? Belongs { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
       
        public int Nature { get; set; } 
        public int LocationStatusResult { get; set; }

    }

public class LedgerAccountIndexDto : LedgerAccountDto
{
    public int Index { get; set; }
    
}
  


public class LedgerAccountWithParent : AuditDto
{
    public Guid? Belongs { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public int Nature { get; set; }
    public int LocationStatusResult { get; set; }
    public virtual List<LedgerAccountWithParent> Sons { get; set; }
}