using ERP.Domain.Entity;
using ERP.Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface IBankServices
    {
        public Task<Banks> InsertBankAccount(Banks obj);
        public Task<List<Banks>> FindBankAccount(PaginationFilter filter);
        public Task<IEnumerable<Banks>> GetAll();
        public Task<Banks> GetBankAccountById(Guid id);
        public Task<Banks>  DeleteBankAccount(Guid id);
        public Task<Banks> UpdateBankAccount(Banks obj);


    }
}
