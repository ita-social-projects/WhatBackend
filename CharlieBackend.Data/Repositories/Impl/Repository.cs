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
        private DbSet<T> entities;
        public Repository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public Task<List<T>> GetAllAsync()
        {
            return entities.AsNoTracking().ToListAsync();
        }

        public async Task<T> InsertAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException();
            await entities.AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            entities.Remove(entity);
        }
    }
}
