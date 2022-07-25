using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entity
{
    public class UserRoll : Audit
    {

        
            public string Email { get; set; }

            [ForeignKey("Rolles")] 

            public Guid RollId { get; set; }

            public virtual Roll Rolles { get; set; }


    }
}

