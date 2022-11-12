using ERP.Domain.Constants;
using ERP.Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class  
    {
        Task<IEnumerable<T>> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate); 
        Task<T> GetById(object id);
        Task<T> Insert(T obj);
        Task<T> FirstOrDefaultAsync();
        Task<List<T>> InsertArray(List<T> obj);
        Task<List<T>> UpdateArray(List<T> obj);
        Task<T> Update(T obj);
        Task<T> Delete(object id);
        void Save();

        Task<int> SaveChangesAsync(); 
    }
}
