using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
