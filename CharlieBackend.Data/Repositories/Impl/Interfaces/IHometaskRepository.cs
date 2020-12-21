using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IHometaskRepository : IRepository<Hometask>
    {
        Task<IList<Hometask>> GetHometasksByCourseId(long courseId);
    }
}
