using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<IList<StudentLessonDto>> GetStudentInfoAsync(long studentId);

        Task<List<Lesson>> GetLessonsForMentorAsync(long? studentGroupId, DateTime? startDate, DateTime? finishDate, long mentorId);

        Task<List<Lesson>> GetAllLessonsForMentorAsync(long mentorId);

        Task<List<Lesson>> GetLessonsByDateAsync(DateTime? startDate, DateTime? finishDate);

        Task<Lesson> GetLastLessonAsync();

        Task<List<Lesson>> GetAllLessonsForStudentGroupAsync(long studentGroupId);

        Task<List<Lesson>> GetLessonsForStudentAsync(long? studentGroupId, DateTime? startDate, DateTime? finishDate, long studentId);

        Task<Lesson> GetLessonByHomeworkIdAsync(long homeworkId);

        Task<Visit> GetVisitByStudentHomeworkIdAsync(long studentHomeworkId);

        Task<bool> DoesLessonWithThemeExistAsync(long themeId);
    }
}
