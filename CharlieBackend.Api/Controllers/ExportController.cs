using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Business.Services.FileServices;
using CharlieBackend.Core.DTO.Export;
using AutoMapper;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage export data from database to .xlsx files
    /// </summary>
    [Route("api/exports")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IExportServiceProvider _exportFactoryService;

        /// <summary>
        /// Export controllers constructor
        /// </summary>
        public ExportController(IExportServiceProvider exportFactoryService,
                                IDashboardService dashboardService)
        {
            _exportFactoryService = exportFactoryService;
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Gets classbook data of every students lesson
        /// </summary>
        /// <param name="request">
        /// 1. Mention "courseId" or "studentGroupId" to filter all course groups or exact student group.
        /// 2. In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of course groups.
        /// 3. "includeAnalytics": ["StudentPresence", "StudentMarks"] params to choose what to return </param>
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("studentsClassbook")]
        [HttpPost]
        public async Task<IActionResult> GetStudentsClassbook([FromBody] StudentsRequestWithFileExtensionDto<ClassbookResultType> request)
        {
            var exportService = _exportFactoryService.GetExportService(request.Extension);

            var results = await _dashboardService
                .GetStudentsClassbookAsync(request.GetStudentsRequestDto());

            var classbook = await exportService.GetStudentsClassbook(results.Data);

            return File(
                classbook.ByteArray,
                classbook.ContentType,
                classbook.Filename);
        }

        /// <summary>
        /// Gets results of every student
        /// </summary>
        /// <param name="request">
        /// 1. Mention "courseId" or "studentGroupId" to filter all course groups or exact student group.
        /// 2. In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of course groups.
        /// 3. "includeAnalytics": ["AverageStudentMark", "AverageStudentVisits"] have to receive params for result to return</param>
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("studentsResults")]
        [HttpPost]
        public async Task<IActionResult> GetStudentsResults([FromBody] StudentsRequestWithFileExtensionDto<StudentResultType> request)
        {
            var exportService = _exportFactoryService.GetExportService(request.Extension);

            var results = await _dashboardService
                .GetStudentsResultAsync(request.GetStudentsRequestDto());

            var studentResults = await exportService.GetStudentsResults(results.Data);

            return File(
                studentResults.ByteArray,
                studentResults.ContentType,
                studentResults.Filename);
        }

        /// <summary>
        /// Gets classbook data of given student
        /// </summary>
        /// <param name="studentId">Student id</param>
        /// <param name="request">In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of students group.
        /// "includeAnalytics": ["StudentPresence", "StudentMarks"] options which report type to receive</param>
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("studentClassbook/{studentId}")]
        [HttpPost]
        public async Task<IActionResult> GetStudentClassbook(long studentId, [FromBody] DashboardAnalyticsRequestWithFileExtensionDto<ClassbookResultType> request)
        {
            var exportService = _exportFactoryService.GetExportService(request.Extension);

            var results = await _dashboardService
                .GetStudentClassbookAsync(studentId, request.GetDashboardAnalyticsRequestDto());

            var studentClassbook = await exportService.GetStudentClassbook(results.Data);

            return File(
                studentClassbook.ByteArray,
                studentClassbook.ContentType,
                studentClassbook.Filename);
        }

        /// <summary>
        /// Gets results data of given student
        /// </summary>
        /// <param name="studentId">Student id</param>
        /// <param name="request">In body you can mention: "startDate", "finishtDate" like optional param to filter 
        /// learning period of students group.
        /// "includeAnalytics": ["AverageStudentMark", "AverageStudentVisits"] have to receive params for data to return</param>
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpPost("studentResults/{studentId}")]
        public async Task<IActionResult> GetStudentResults(long studentId, [FromBody] DashboardAnalyticsRequestWithFileExtensionDto<StudentResultType> request)
        {
            var exportService = _exportFactoryService.GetExportService(request.Extension);

            var results = await _dashboardService
                .GetStudentResultAsync(studentId, request.GetDashboardAnalyticsRequestDto());

            var studentResults = await exportService.GetStudentResults(results.Data);

            return File(
                studentResults.ByteArray,
                studentResults.ContentType,
                studentResults.Filename);
        }

        /// <summary>
        /// Gets report data of student group results
        /// </summary>
        /// <param name="courseId">Course id</param>
        /// <param name="request">In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of students group.
        /// "includeAnalytics": ["AverageStudentGroupMark", "AverageStudentGroupVisitsPercentage"] have to receive params for data to return</param>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost("studentGroupResults/{courseId}")]
        public async Task<IActionResult> GetStudentGroupResults(long courseId, [FromBody] DashboardAnalyticsRequestWithFileExtensionDto<StudentGroupResultType> request)
        {
            var exportService = _exportFactoryService.GetExportService(request.Extension);

            var results = await _dashboardService
            .GetStudentGroupResultAsync(courseId, request.GetDashboardAnalyticsRequestDto());

            var studentGroupResults = await exportService.GetStudentGroupResults(results.Data);

            return File(
                studentGroupResults.ByteArray,
                studentGroupResults.ContentType,
                studentGroupResults.Filename);
        }
    }
}
