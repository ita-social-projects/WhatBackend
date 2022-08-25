using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<long>> GetGroupsIdsByCourseIdAndPeriodAsync(long courseId, DateTime? startDate, DateTime? finishDate);

        Task<List<StudentVisitDto>> GetStudentsPresenceListByGroupIdsAndDate(IEnumerable<long> studentGroupIds,
            DateTime? startDate, DateTime? finishDate);

        Task<List<StudentMarkDto>> GetStudentsMarksListByGroupIdsAndDate(IEnumerable<long> studentGroupIds,
            DateTime? startDate, DateTime? finishDate);

        Task<List<long>> GetGroupsIdsByCourseIdAsync(long courseId);

        Task<List<long>> GetStudentsIdsByGroupIdsAsync(IEnumerable<long> studentGroupId);

        Task<List<AverageStudentMarkDto>> GetStudentAvgMarksAsync(IEnumerable<long> studentIds, IEnumerable<long> studentGroupsIds);

        Task<List<AverageStudentVisitsDto>> GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(IEnumerable<long> studentIds, IEnumerable<long> studentGroupIds);

        Task<List<StudentVisitDto>> GetStudentsPresenceListByStudentIds(IEnumerable<long> studentIds);

        Task<List<StudentMarkDto>> GetStudentsMarksListByStudentIds(IEnumerable<long> studentIds);

        Task<List<long>> GetGroupsIdsByStudentIdAndPeriodAsync(long studentId, DateTime? startDate, DateTime? finishDate);

        Task<List<AverageStudentVisitsDto>> GetStudentAverageVisitsPercentageByStudentIdsAsync(long studentId, List<long> studentGroupsIds);

        Task<List<StudentVisitDto>> GetStudentPresenceListByStudentIds(long studentId, List<long> studentGroupsIds);

        Task<List<StudentMarkDto>> GetStudentMarksListByStudentIds(long studentId, List<long> studentGroupsIds);

        Task<List<AverageStudentGroupMarkDto>> GetStudentGroupsAverageMarks(List<long> studentGroupIds);

        Task<List<AverageStudentGroupVisitDto>> GetStudentGroupsAverageVisits(List<long> studentGroupIds);

        Task<List<AverageStudentMarkDto>> GetStudentHomeworkAvgMarksAsync(IEnumerable<long> studentIds,
            IEnumerable<long> studentGroupsIds);
    }
}
