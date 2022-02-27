using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class BoxBalanceTransportEdit
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public DateTime Date { get; set; }
    }
}
