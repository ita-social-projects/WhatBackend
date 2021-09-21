using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using System.Linq;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IHomeworkRepository : IRepository<Homework>
    {
        void UpdateManyToMany(IEnumerable<AttachmentOfHomework> currentHomeworkAttachments,
                     IEnumerable<AttachmentOfHomework> newHomeworkAttachments);

        IQueryable<Homework> GetHomeworksWithThemeNameAndAtachemntsQuery();

        Task<IList<Homework>> GetHomeworksByLessonId(long studentGroupId);

        Task<Homework> GetMentorHomeworkAsync(long mentorId, long homeworkId);
    }
}
