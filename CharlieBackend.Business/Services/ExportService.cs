using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Business.Services.Interfaces;

namespace CharlieBackend.Business.Services
{
    public class ExportService : IExportService
    {
        private readonly IDashboardService _dashboardService;

        public ExportService (IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<ClassbookExport> GetStudentsClassbook(StudentsRequestDto<ClassbookResultType> request)
        {
            var results = await _dashboardService
                .GetStudentsClassbookAsync(request);

            var classbook = new ClassbookExport();

            await classbook.FillFile(results.Data);
            classbook.AdjustContent();

            return classbook;
        }

        public async Task<StudentsResultsExport> GetStudentsResults(StudentsRequestDto<StudentResultType> request)
        {
            var results = await _dashboardService
                .GetStudentsResultAsync(request);

            var result = new StudentsResultsExport();

            await result.FillFile(results.Data);
            result.AdjustContent();

            return result;
        }

        public async Task<StudentClassbook> GetStudentClassbook(long studentId, DashboardAnalyticsRequestDto<ClassbookResultType> request)
        {
            var results = await _dashboardService
                .GetStudentClassbookAsync(studentId, request);

            var result = new StudentClassbook();

            await result.FillFile(results.Data);
            result.AdjustContent();

            return result;
        }

        public async Task<StudentResult> GetStudentResults(long studentId, DashboardAnalyticsRequestDto<StudentResultType> request)
        {
            var results = await _dashboardService
                .GetStudentResultAsync(studentId, request);

            var result = new StudentResult();

            await result.FillFile(results.Data);
            result.AdjustContent();

            return result;
        }

        public async Task<StudentGroupResults> GetStudentGroupResults(long courseId, DashboardAnalyticsRequestDto<StudentGroupResultType> request)
        {
            var results = await _dashboardService
            .GetStudentGroupResultAsync(courseId, request);

            var result = new StudentGroupResults();

            await result.FillFile(results.Data);
            result.AdjustContent();

            return result;
        }
    }
}
