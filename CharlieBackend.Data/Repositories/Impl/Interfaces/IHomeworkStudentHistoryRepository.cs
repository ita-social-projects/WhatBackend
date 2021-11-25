using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IHomeworkStudentHistoryRepository : IRepository<HomeworkStudentHistory>
    {
        Task<IList<HomeworkStudentHistory>> GetHomeworkStudentHistoryByHomeworkStudentId(long homeworkStudentId);
    }
}
