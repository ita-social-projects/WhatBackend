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
        Task<List<long>> GetGroupsIdsByCourseIdAsync(long courseId, DateTime? startDate, DateTime? finishDate);

        Task<List<long>> GetStudentsIdsByGroupIdsAsync(IEnumerable<long> studentGroupId);

        Task<List<AverageStudentMarkDto>> GetStudentsAverageMarksByStudentIdsAsync(IEnumerable<long> studentIds);

        Task<List<AverageStudentVisitsDto>> GetStudentsAverageVisitsByStudentIdsAsync(IEnumerable<long> studentIds);

        Task<List<StudentVisitDto>> GetStudentsPresenceListByStudentIds(IEnumerable<long> studentIds);

        Task<List<StudentMarkDto>> GetStudentsMarksListByStudentIds(IEnumerable<long> studentIds);

        Task<List<long>> GetGroupsIdsByStudentIdAndPeriodAsync(long studentId, DateTime? startDate, DateTime? finishDate);

        Task<List<AverageStudentMarkDto>> GetStudentAverageMarksByStudentIdAsync(long studentId, List<long> studentGroupsIds);

        Task<List<AverageStudentVisitsDto>> GetStudentAverageVisitsPercentageByStudentIdsAsync(long studentId, List<long> studentGroupsIds);

        Task<List<StudentVisitDto>> GetStudentPresenceListByStudentIds(long studentId, List<long> studentGroupsIds);

        Task<List<StudentMarkDto>> GetStudentMarksListByStudentIds(long studentId, List<long> studentGroupsIds);

        Task<List<AverageStudentGroupMarkDto>> GetStudentGroupsAverageMarks(List<long> studentGroupIds);

        Task<List<AverageStudentGroupVisitDto>> GetStudentGroupsAverageVisits(List<long> studentGroupIds);
    }
}
