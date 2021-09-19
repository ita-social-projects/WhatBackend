using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IHomeworkStudentHistoryRepository : IRepository<HomeworkStudentHistory>
    {
        Task<IList<HomeworkStudentHistory>> GetHomeworkStudentHistoryByHomeworkStudentId(long homeworkStudentId);
        Task<HomeworkStudentHistory> GetHomeworkStudentHistory(long groupId, long studentId, long homeworkId);
    }
}
