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
        private DbSet<T> _entities;

        public Repository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _entities = applicationContext.Set<T>();
        }

        public IQueryable<T> GetQueryableNoTracking()
        {
            return _entities.AsNoTracking();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async virtual Task<T> GetByIdAsync(long id)
        {
            return await _entities.FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            _entities.Add(entity);
        }

        public void Update(T updatedEntity)
        {
            _entities.Update(updatedEntity);
        }

        public async Task DeleteAsync(long id)
        {
            var found = await _entities.FirstOrDefaultAsync(entity => entity.Id == id);

            if (found != null)
            {
                _entities.Remove(found);
            }
        }

        public async Task<bool> IsEntityExistAsync(long id)
        {
            return await _entities.AnyAsync(x => x.Id == id);
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
