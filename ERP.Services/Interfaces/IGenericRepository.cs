using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(object id);
        Task<T> Insert(T obj);
        Task<List<T>> InsertArray(List<T> obj);
        Task<List<T>> UpdateArray(List<T> obj);
        Task<T> Update(T obj);
        Task<T> Delete(object id);
        void Save();

        Task<int> SaveChangesAsync();
    }
}
