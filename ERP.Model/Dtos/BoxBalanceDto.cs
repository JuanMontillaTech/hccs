using System;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos;

public class BoxBalanceDto :AuditDto
{
 
       public decimal? Balance { get; set; } = 0;
 
        public DateTime MonthBalance { get; set; } 

}
