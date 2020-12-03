using CharlieBackend.Core.DTO.Dashboard;
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
        public Task<List<long>> GetGroupsIdsByCourseIdAsync(long CourseId, DateTime startDate, DateTime finishDate);

        public Task<List<long>> GetStudentsIdsByGroupIdsAsync(IEnumerable<long> studentGroupId);

        public Task<List<AverageStudentMarkDto>> GetStudentsAverageMarksByStudentIdsAsync(IEnumerable<long> studentIds);

        public Task<List<AverageStudentVisitsDto>> GetStudentsAverageVisitsByStudentIdsAsync(IEnumerable<long> studentIds);

        public List<AverageStudentVisitsDto> GetStudentsAverageVisitsByStudentsVisits(List<StudentVisitDto> studentsVisits);

        public Task<List<StudentVisitDto>> GetStudentsPresenceListByStudentIds(IEnumerable<long> studentIds);

        public Task<List<StudentMarkDto>> GetStudentsMarksListByStudentIds(IEnumerable<long> studentIds);

        public Task<List<long>> GetGroupsIdsByStudentIdAndPeriodAsync(long studentId, DateTime startDate, DateTime finishDate);

        public Task<List<AverageStudentMarkDto>> GetStudentAverageMarksByStudentIdAsync(long studentId, List<long> studentGroupsIds);

        public Task<List<AverageStudentVisitsDto>> GetStudentAverageVisitsPercentageByStudentIdsAsync(long studentId, List<long> studentGroupsIds);

        public Task<List<StudentVisitDto>> GetStudentPresenceListByStudentIds(long studentId, List<long> studentGroupsIds);

        public Task<List<StudentMarkDto>> GetStudentMarksListByStudentIds(long studentId, List<long> studentGroupsIds);

        public Task<List<AverageStudentGroupMarkDto>> GetStudentGroupsAverageMarks(List<long> studentGroupIds);

        public Task<List<AverageStudentGroupVisitDto>> GetStudentGroupsAverageVisits(List<long> studentGroupIds);
    }
}
