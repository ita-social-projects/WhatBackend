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
        private DbSet<T> _entities;
        protected readonly ApplicationContext _applicationContext;

        public Repository(ApplicationContext applicationContext)
        {
            _entities = applicationContext.Set<T>();
            _applicationContext = applicationContext;
        }

        public Task<List<T>> GetAllAsync()
        {
            return _entities.ToListAsync();
        }

        public Task<T> GetByIdAsync(long id)
        {
            return _entities.FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException();
            _entities.Add(entity);
        }

        public void Update(T updatedEntity)
        {
            _entities.Update(updatedEntity);
        }

        public async Task DeleteAsync(long id)
        {
            var found = await _entities.FirstOrDefaultAsync(entity => entity.Id == id);
            if (found != null) _entities.Remove(found);
        }
    }
}
