using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class BoxBalanceTransportCreate
    {

        public decimal Balance { get; set; }
        public DateTime MonthBalance { get; set; }
    }
}
