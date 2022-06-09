using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<List<Lesson>> GetLessonsForMentorAsync(long? studentGroupId, DateTime? startDate, DateTime? finishDate, long mentorId);

        Task<List<Lesson>> GetAllLessonsForMentor(long mentorId);

        Task<List<Lesson>> GetLessonsByDate(DateTime? startDate, DateTime? finishDate);

        Task<Lesson> GetLastLesson();

        Task<List<Lesson>> GetAllLessonsForStudentGroup(long studentGroupId);

        Task<List<Lesson>> GetLessonsForStudentAsync(long? studentGroupId, DateTime? startDate, DateTime? finishDate, long studentId);

        Task<Lesson> GetLessonByHomeworkId(long homeworkId);

        Task<Visit> GetVisitByStudentHomeworkIdAsync(long studentHomeworkId);
        Task<bool> DoesLessonWithThemeExist(long themeId);
    }
}
