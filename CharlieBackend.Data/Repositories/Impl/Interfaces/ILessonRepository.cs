using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Lesson;
using System;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<IList<StudentLessonDto>> GetStudentInfoAsync(long studentId);

        Task<List<Lesson>> GetLessonsForMentorAsync(long? studentGroupId, DateTime? startDate, DateTime? finishDate, long mentorId);

        Task<List<Lesson>> GetAllLessonsForMentor(long mentorId);

        Task<List<Lesson>> GetLessonsForStudentAsync(long? studentGroupId, DateTime? startDate, DateTime? finishDate, long studentId);

        Task<Lesson> GetLessonByHomeworkId(long homeworkId);

        Task<Visit> GetVisitByStudentHomeworkIdAsync(long studentHomeworkId);
        Task<IList<long?>> GetStudentGroupsOfThemesAsync(long themeId);
    }
}
