using System;
using System.Threading.Tasks;
using ERP.Domain.Entity;

namespace ERP.Services;

public interface IAccountingProcess
{
  public Task PostJournalEntry(Transactions transactions);
  public Task UpdateJournalEntry(Transactions transactions);
  public Task DeleteJournalEntry(Transactions transactions);
  public Task PostJournaSellTransactionReceipt(TransactionReceipt _TransactionReceipt);
}
