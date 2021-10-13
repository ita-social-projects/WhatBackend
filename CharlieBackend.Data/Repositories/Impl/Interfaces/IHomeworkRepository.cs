using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Homework;
using System.Linq;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IHomeworkRepository : IRepository<Homework>
    {
        void UpdateManyToMany(IEnumerable<AttachmentOfHomework> currentHomeworkAttachments,
                     IEnumerable<AttachmentOfHomework> newHomeworkAttachments);

        IQueryable<Homework> GetHomeworksWithThemeNameAndAtachemntsQuery();

        Task<IList<Homework>> GetHomeworksByLessonId(long studentGroupId);
        Task<IList<Homework>> GetHomeworks(GetHomeworkRequestDto request);
        Task<Homework> GetMentorHomeworkAsync(long mentorId, long homeworkId);
        Task<IList<Homework>> GetHomeworksForMentor(GetHomeworkRequestDto request, long mentorId);
        Task<IList<Homework>> GetHomeworksForStudent(GetHomeworkRequestDto request, long studentId);
        Task<IList<Homework>> GetNotDoneHomeworksByStudentGroup(long studentGroupId, long studentId, DateTime? dueDate);
    }
}
