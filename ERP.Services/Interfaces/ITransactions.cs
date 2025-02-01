using ERP.Domain.Command;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface ITransactionService
    {
        public Task<Transactions> TransactionProcess(Transactions transactionsDto, Guid formId);
        public Task<RecipePayDto> TransactionReceiptProcess(RecipePayDto RecipePayDto);
        public  Task<Transactions> CloneTransaction(Guid id,Guid formId, string Commentary = null); 
        public Task<bool> UpdateReceiptStatusAsync(List<Guid> receiptIds, Guid newStatusId);

    }
}
