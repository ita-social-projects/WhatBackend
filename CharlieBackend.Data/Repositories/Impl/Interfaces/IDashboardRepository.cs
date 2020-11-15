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
        public Task<List<long>> GetGroupsIdsByCourceIdAsync(long courceId, DateTime startDate);

        public Task<List<long>> GetStudentsIdsByGroupIdAsync(long studentGroupId);

        public Task<List<long>> GetStudentsIdsByGroupIdsAsync(IEnumerable<long> studentsIds);

        public Task<List<AverageStudentMarkDto>> GetStudentsAverageMarksByStudentIdsAsync(IEnumerable<long> studentIds);

        public Task<List<AverageStudentVisitsDto>> GetStudentsAverageVisitsByStudentIdsAsync(IEnumerable<long> studentIds);

        public List<AverageStudentVisitsDto> GetStudentsAverageVisitsByStudentsVisits(List<StudentVisitDto> studentsVisits);

        public Task<List<StudentVisitDto>> GetStudentsPresenceListByStudentIds(IEnumerable<long> studentIds);

        public Task<List<StudentMarkDto>> GetStudentsMarksListByStudentIds(IEnumerable<long> studentIds);
    }
}
