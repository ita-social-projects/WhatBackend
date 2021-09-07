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

        public async Task<ClassbookExportXlsx> GetStudentsClassbook(StudentsRequestDto<ClassbookResultType> request)
        {
            var results = await _dashboardService
                .GetStudentsClassbookAsync(request);

            var classbook = new ClassbookExportXlsx();

            await classbook.FillFile(results.Data);
            classbook.AdjustContent();

            return classbook;
        }

        public async Task<StudentsResultsExportXlsx> GetStudentsResults(StudentsRequestDto<StudentResultType> request)
        {
            var results = await _dashboardService
                .GetStudentsResultAsync(request);

            var result = new StudentsResultsExportXlsx();

            await result.FillFile(results.Data);
            result.AdjustContent();

            return result;
        }

        public async Task<StudentClassbookXlsx> GetStudentClassbook(long studentId, DashboardAnalyticsRequestDto<ClassbookResultType> request)
        {
            var results = await _dashboardService
                .GetStudentClassbookAsync(studentId, request);

            var result = new StudentClassbookXlsx();

            await result.FillFile(results.Data);
            result.AdjustContent();

            return result;
        }

        public async Task<StudentResultXlsx> GetStudentResults(long studentId, DashboardAnalyticsRequestDto<StudentResultType> request)
        {
            var results = await _dashboardService
                .GetStudentResultAsync(studentId, request);

            var result = new StudentResultXlsx();

            await result.FillFile(results.Data);
            result.AdjustContent();

            return result;
        }

        public async Task<StudentGroupResultsXlsx> GetStudentGroupResults(long courseId, DashboardAnalyticsRequestDto<StudentGroupResultType> request)
        {
            var results = await _dashboardService
            .GetStudentGroupResultAsync(courseId, request);

            var result = new StudentGroupResultsXlsx();

            await result.FillFile(results.Data);
            result.AdjustContent();

            return result;
        }
    }
}
