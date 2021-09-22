using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class MentorOfCourseRepository : Repository<MentorOfCourse>, IMentorOfCourseRepository
    {
        public MentorOfCourseRepository(ApplicationContext applicationContext) 
            : base(applicationContext) 
        {
        }

        public async Task<List<MentorOfCourse>> GetAllMentorCoursesAsync(long mentorId)
        {
            return await _applicationContext.MentorsOfCourses
                    .Where(mentorOfCourse => mentorOfCourse.MentorId == mentorId)
                    .ToListAsync();
        }

        public async Task<MentorOfCourse> GetMentorOfCourseIdAsync(MentorOfCourse mentorOfCourse)
        {
            var foundMentorOfCourse = await _applicationContext.MentorsOfCourses
                    .FirstOrDefaultAsync(mOc => mOc.CourseId == mentorOfCourse.CourseId &&
                                         mOc.MentorId == mentorOfCourse.MentorId);

            if (foundMentorOfCourse == null) 
            {
                return mentorOfCourse;
            }
            
            mentorOfCourse.Id = foundMentorOfCourse.Id;

            return mentorOfCourse;
        }
        public async Task<long> GetCoursesAsync(long courseId)
        {
            return await _applicationContext.MentorsOfCourses
                .Where(x => x.CourseId == courseId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }
    }
}
