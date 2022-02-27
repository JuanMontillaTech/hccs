using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Entity
{
    public class Transaction : Audit
    {
        public Guid NumerationId { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public Guid? ContactId { get; set; }
        public Guid BankId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public int TransactioTypeId { get; set; } = 1;
        public int TransactioStatusId { get; set; } = 1;
        public string Commentary { get; set; }
        public decimal Value { get; set; }
        public virtual ICollection<TransactionDetails> TransactionDetails { get; set; }

        public virtual Numeration Numeration { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Bank Banks { get; set; }
        public virtual PaymentMethod PaymentMethods { get; set; }


    }
    public class TransactionDetails : Audit
    {
     
        public Guid TransactionId { get; set; }
        public Guid LedgerAccountId { get; set; }
        public Guid TaxesId { get; set; }
        public decimal Value { get; set; }
        public decimal Amount { get; set; }
        public string Commentary { get; set; }
        public virtual Taxes Taxes { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual LedgerAccount LedgerAccount { get; set; }

    }
}
