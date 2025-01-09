using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.Implementations
{
    public class BankServices : IBankServices
    {
        private readonly IGenericRepository<Banks> _repository;

        public BankServices(IGenericRepository<Banks> repository)
        {
            _repository = repository;
        }

        public async Task<Banks> InsertBankAccount(Banks obj)
        {
            return await _repository.InsertAsync(obj);
        }

        public async Task<List<Banks>> FindBankAccount(PaginationFilter filter)
        {
            return await _repository.Find(x => x.IsActive == true
                && (x.AccountNumber.ToLower().Contains(filter.Search.Trim().ToLower()))
                && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).ToListAsync();
        }

        public async Task<IEnumerable<Banks>> GetAll()
        {
            return await _repository.Find(x => x.IsActive).AsQueryable()
                .Include(x => x.Currencys).Include(x => x.LedgerAccount).ToListAsync();
        }

        public async Task<Banks> GetBankAccountById(Guid id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Banks> DeleteBankAccount(Guid id)
        {
            var data = await _repository.GetById(id);
            if (data == null)
            {
                // Manejar el caso donde no se encuentra el banco
                return null;
            }
            data.IsActive = false;
            await _repository.Update(data);
            return data;
        }

        public async Task<Banks> UpdateBankAccount(Banks obj)
        {
            obj.IsActive = true; // Asegurar que la cuenta esté activa al actualizar
            return await _repository.Update(obj);
        }
    }
}