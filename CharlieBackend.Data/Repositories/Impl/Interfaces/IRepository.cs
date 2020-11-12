using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetQueryableNoTracking();

        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(long id);

        void Add(T entity);

        void Update(T entity);

        Task DeleteAsync(long id);
    }
}
