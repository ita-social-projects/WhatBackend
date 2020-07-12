using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        public Task<bool> IsCourseNameTakenAsync(string courseName);
        public Task<List<Course>> GetCoursesByIdsAsync(List<long> courseIds);
    }
}
