using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _applicationContext;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _entities = applicationContext.Set<T>();
        }

        public IQueryable<T> GetQueryableNoTracking()
        {
            return _entities.AsNoTracking();
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return _entities.ToListAsync();
        }

        public virtual Task<T> GetByIdAsync(long id)
        {
            return _entities.FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public async Task DeleteAsync(long id)
        {
            var found = await _entities.FirstOrDefaultAsync(entity => entity.Id == id);

            if (found != null)
            {
                _entities.Remove(found);
            }
        }

        public Task<bool> IsEntityExistAsync(long id)
        {
            return _entities.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<long>> GetNotExistEntitiesIdsAsync(IEnumerable<long> ids)
        {
            var existIds = await _entities.Where(entity => ids.Contains(entity.Id))
                                          .Select(entity => entity.Id)
                                          .ToListAsync();

            return ids.Except(existIds);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _entities.AddRange(entities);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _entities.UpdateRange(entities);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }
    }
}