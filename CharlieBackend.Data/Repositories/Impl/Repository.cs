using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _applicationContext;
        private DbSet<T> _entities;
        public Repository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _entities = applicationContext.Set<T>();
        }

        public Task<List<T>> GetAllAsync()
        {
            return _entities.AsNoTracking().ToListAsync();
        }

        public Task<T> GetByIdAsync(long id)
        {
            return _entities.FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task<T> PostAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException();
            await _entities.AddAsync(entity);
            return entity;
        }

        public async Task Delete(long id)
        {
            var found = await _entities.FirstOrDefaultAsync(entity => entity.Id == id);
            if (found != null) _entities.Remove(found);
        }
    }
}
