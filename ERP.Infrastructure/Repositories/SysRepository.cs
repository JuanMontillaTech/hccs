using ERP.Infrastructure.DBContexts;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERP.Infrastructure.Repositories
{
    public class SysRepository<T> : ISysRepository<T> where T : class
    {
        private SysDbContext _context = null;
        private DbSet<T> _table = null;

        public SysRepository(SysDbContext context)
        {
            this._context = context;
            _table = _context.Set<T>();
        }

        public async Task<T> Delete(object id)
        {
            T existing = _table.Find(id);
            await Task.Run(() => _context.Entry(existing).CurrentValues.SetValues(existing)).ConfigureAwait(false);
            _context.Entry<T>(existing).State = EntityState.Deleted;
            return Task.FromResult(existing).Result;
        }


        public async Task<IEnumerable<T>> GetAll()
        {
            var result = await _table.ToListAsync();
            return result;
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            var result = await _table.FirstOrDefaultAsync();
            return result;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _table.Where(predicate).AsQueryable();
        }


        public async Task<T> GetById(object id) => await _table.FindAsync(id);


        public async Task<T> Inactive(object id)
        {
            T existing = await _table.FindAsync(id);

            if (existing == null)
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }

            // Supongamos que la entidad tiene una propiedad llamada IsInactive
            // y la marcamos como inactiva en lugar de eliminarla físicamente.
            var property = existing.GetType().GetProperty("IsActive");
    
            if (property != null && property.PropertyType == typeof(bool))
            {
                property.SetValue(existing, true);
                _context.Entry(existing).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return existing;
            }
            else
            {
                throw new InvalidOperationException($"Entity of type {typeof(T).Name} does not have a valid property for inactivation.");
            }
        }

        public async Task<T> InsertAsync(T obj)
        {
            await _table.AddAsync(obj);
            return obj;
        }

        public void Insert(T obj)
        {
            _table.Add(obj);
        }

        public async Task<List<T>> InsertArray(List<T> obj)
        {
            await _table.AddRangeAsync(obj);
            return obj;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<T> Update(T obj)
        {
            await Task.Run(() => _context.Entry(obj).CurrentValues.SetValues(obj)).ConfigureAwait(false);
            _context.Entry(obj).State = EntityState.Modified;
            return Task.FromResult(obj).Result;
        }

        public async Task<List<T>> UpdateArray(List<T> Listobj)
        {
            foreach (var obj in Listobj)
            {
                await Task.Run(() => _context.Entry(obj).CurrentValues.SetValues(obj)).ConfigureAwait(false);
                _context.Entry(obj).State = EntityState.Modified;
            }

            return Task.FromResult(Listobj).Result;
        }
    }
}