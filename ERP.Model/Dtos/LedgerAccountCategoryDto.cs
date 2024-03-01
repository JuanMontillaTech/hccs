
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos
{
    public class LedgerAccountCategoryDto : AuditDto
    {
        public string Name { get; set; }
       
    }

}
