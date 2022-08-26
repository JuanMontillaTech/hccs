using ERP.Domain.Command;
using ERP.Domain.Dtos;
using ERP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        public Task<Result<TransactionsDto>> Save(TransactionsDto transactionsDto)
        {
            throw new NotImplementedException();
        }
    }
}
