using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Model.Entity
{
    public class Transaction : Audit
    {
       
        public string Reference { get; set; }
        public string Commentary { get; set; } 
        public DateTime Date { get; set; }
 
        public virtual ICollection<TransactionDetails> TransactionDetails { get; set; }


    }
    public class TransactionDetails : Audit
    {
        public int Contact { get; set; }
        public Guid TransactionId { get; set; } 
        public string Commentary { get; set; }
        public virtual Transaction Transaction { get; set; }

    }
}
