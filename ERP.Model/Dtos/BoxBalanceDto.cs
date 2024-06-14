using System;
using ERP.Domain.Command;

namespace ERP.Domain;

public class BoxBalanceDto :AuditDto
{

        public decimal Balance { get; set; }
 
        public DateTime MonthBalance { get; set; } 

}
