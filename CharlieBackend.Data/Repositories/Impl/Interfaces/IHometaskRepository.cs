using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IHometaskRepository : IRepository<Hometask>
    {
        Task<IList<Hometask>> GetHometasksByCourseId(long studentId);
    }
}
