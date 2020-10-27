using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Lesson;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(ApplicationContext applicationContext) 
            : base(applicationContext) 
        { 
        }

        public new Task<List<Lesson>> GetAllAsync()
        {
            return _applicationContext.Lessons
                    .Include(lesson => lesson.Visits)
                    .Include(lesson => lesson.Theme)
                    .ToListAsync();
        }

        public async Task<IList<StudentLessonModel>> GetStudentInfoAsync(long studentId)
        {
            try
            {
                var studentLessonModels = new List<StudentLessonModel>();

                var visits = await _applicationContext.Visits
                        .Include(visit => visit.Lesson)
                        .ThenInclude(lesson => lesson.Theme)
                        .Where(visit => visit.StudentId == studentId).ToListAsync();

                for (int i = 0; i < visits.Count; i++)
                {
                    var studentLessonModel = new StudentLessonModel
                    {
                        Id = visits[i].Lesson.Id,
                        Comment = visits[i].Comment,
                        Mark = visits[i].StudentMark,
                        Presence = visits[i].Presence,
                        ThemeName = visits[i].Lesson.Theme.Name,
                        LessonDate = visits[i].Lesson.LessonDate,
                        StudentGroupId = visits[i].Lesson.StudentGroupId
                    };

                    studentLessonModels.Add(studentLessonModel);
                }

                return studentLessonModels;
            }
            catch 
            {
                return null; 
            }
        }
    }
}
