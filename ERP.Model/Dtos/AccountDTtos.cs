using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
  public  class AccountDTtos
    {
        public Guid Id { get; set; }
        public string Icon { get; set; }

        public string Text { get; set; }
        public string Description { get; set; }

        public string AccountCode { get; set; }
        public List<AccountDTtos> Children { get; set; }

    }
}
