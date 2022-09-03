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
        public Task<Transactions> TransactionProcess(Transactions transactionsDto);
         
    }
}
