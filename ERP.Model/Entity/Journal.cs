using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP.Domain.Entity
{
    public class Journal : Audit
    {
        public string Code { get; set; }
        public string Reference { get; set; }
        public string Commentary { get; set; }
        public Guid? TypeRegisterId { get; set; }
        public DateTime Date { get; set; }
        
        public virtual TypeRegister TypeRegister { get; set; }

        public virtual ICollection<JournaDetails> JournaDetails { get; set; }

    }
    public class JournaDetails : Audit
    {
        public Guid? ContactId { get; set; }
        public Guid JournalId { get; set; }
        [ForeignKey("LedgerAccount")]
        public Guid? LedgerAccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public virtual LedgerAccount LedgerAccount { get; set; }


    }
}
