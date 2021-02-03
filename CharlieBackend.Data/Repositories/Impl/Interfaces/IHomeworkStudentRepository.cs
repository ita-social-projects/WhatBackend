using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IHomeworkStudentRepository : IRepository<HomeworkStudent>
    {
        Task<bool> IsStudentHasHomeworkAsync(long studentId, long homeworkId);

        Task<IList<HomeworkStudent>> GetHomeworkStudentForStudentByStudentId(long id);

        Task<IList<HomeworkStudent>> GetHomeworkStudentForMentorByHomeworkId(long homeworkId);

        void UpdateManyToMany(IEnumerable<AttachmentOfHomeworkStudent> currentHomeworkAttachments,
                            IEnumerable<AttachmentOfHomeworkStudent> newHomeworkAttachments);
    }
}
