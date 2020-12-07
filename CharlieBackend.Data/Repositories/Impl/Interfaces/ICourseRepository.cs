using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        public Task<bool> IsCourseNameTakenAsync(string courseName);

        public Task<List<Course>> GetCoursesByIdsAsync(List<long> courseIds);

        Task<Result<bool>> DisableCourseByIdAsync(long id);

        Task<bool> IsCourseEmptyAsync(long id);
    }
}
