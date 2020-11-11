using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IDashboardRepository
    {
        public Task<List<StudentGroup>> GetAllStudentGroupsAsync();

        public Task<List<Lesson>> GetStudentGroupLessonsAsync(long? studentGroupId);

        public Task<StudentGroup> GetStudentGroupByIdAsync(long studentGroupId);

        public Task<List<StudentOfStudentGroup>> GetStudentsOfStudentGroup(long studentGroupId);

        public Task<List<Visit>> GetStudentGroupLessonVisitsAsync(long lessonId);

        public Task<List<Visit>> GetStudentVisitsAsync(long lessonId);

        public Task<List<Visit>> GetStudentVisitsByStudentIdAndStudGroupAsync(long studentGroupId, long studentId);

        public Task<List<StudentOfStudentGroup>> GetStudentGroupsAsync(long studentId);

        public Task<Visit> GetStudentVisitByLessonIdAndStudentId(long studentId, long lessonId);

        public Task<List<StudentGroup>> GetStudentGroupsByCourceIdAsync(long courceId);
    }
}
