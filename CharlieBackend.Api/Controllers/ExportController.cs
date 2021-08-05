using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Business.Services.Interfaces;

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

        /// <summary>
        /// Export controllers constructor
        /// </summary>
        public ExportController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Gets classbook data of every students lesson
        /// </summary>
        /// <param name="request">
        /// 1. Mention "courseId" or "studentGroupId" to filter all course groups or exact student group.
        /// 2. In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of cource groups.
        /// 3. "includeAnalytics": ["StudentPresence", "StudentMarks"] params to choose what to return </param>
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("studentsClassbook")]
        [HttpGet]
        public async Task<FileResult> GetStudentsClassbook([FromBody] StudentsRequestDto<ClassbookResultType> request)
        {
            var results = await _dashboardService
                .GetStudentsClassbookAsync(request);

            var classbook = new ClassbookExport();

            await classbook.FillFile(results.Data);
            classbook.AdjustContent();

            return File(await
                classbook.GetByteArrayAsync(),
                classbook.GetContentType(),
                classbook.GetFileName());
        }

        /// <summary>
        /// Gets results of every student
        /// </summary>
        /// <param name="request">
        /// 1. Mention "courseId" or "studentGroupId" to filter all cource groups or exact student group.
        /// 2. In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of cource groups.
        /// 3. "includeAnalytics": ["AverageStudentMark", "AverageStudentVisits"] have to receive params for result to return</param>
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("studentsResults")]
        [HttpGet]
        public async Task<FileResult> GetStudentsResults([FromBody] StudentsRequestDto<StudentResultType> request)
        {
            var results = await _dashboardService
                .GetStudentsResultAsync(request);

            var result = new StudentsResultsExport();

            await result.FillFile(results.Data);
            result.AdjustContent();

            return File(await
                result.GetByteArrayAsync(),
                result.GetContentType(),
                result.GetFileName());
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
        [HttpGet]
        public async Task<FileResult> GetStudentClassbook(long studentId, [FromBody] DashboardAnalyticsRequestDto<ClassbookResultType> request)
        {
            var results = await _dashboardService
                .GetStudentClassbookAsync(studentId, request);

            var result = new StudentClassbook();

            await result.FillFile(results.Data);
            result.AdjustContent();

            return File(await
                result.GetByteArrayAsync(),
                result.GetContentType(),
                result.GetFileName());
        }

        /// <summary>
        /// Gets results data of given student
        /// </summary>
        /// <param name="studentId">Student id</param>
        /// <param name="request">In body you can mention: "startDate", "finishtDate" like optional param to filter 
        /// learning period of students group.
        /// "includeAnalytics": ["AverageStudentMark", "AverageStudentVisits"] have to receive params for data to return</param>
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet("studentResults/{studentId}")]
        public async Task<FileResult> GetStudentResults(long studentId, [FromBody] DashboardAnalyticsRequestDto<StudentResultType> request)
        {
            var results = await _dashboardService
                .GetStudentResultAsync(studentId, request);

            var result = new StudentResult();

            await result.FillFile(results.Data);
            result.AdjustContent();

            return File(await
                result.GetByteArrayAsync(),
                result.GetContentType(),
                result.GetFileName());
        }

        /// <summary>
        /// Gets report data of student group results
        /// </summary>
        /// <param name="courseId">Course id</param>
        /// <param name="request">In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of students group.
        /// "includeAnalytics": ["AverageStudentGroupMark", "AverageStudentGroupVisitsPercentage"] have to receive params for data to return</param>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet("studentGroupResults/{courseId}")]
        public async Task<FileResult> GetStudentGroupResults(long courseId, [FromBody] DashboardAnalyticsRequestDto<StudentGroupResultType> request)
        {
            var results = await _dashboardService
            .GetStudentGroupResultAsync(courseId, request);

            var result = new StudentGruopResults();

            result.FillFile(results.Data);
            result.AdjustContent();

            return File(await
                result.GetByteArrayAsync(),
                result.GetContentType(),
                result.GetFileName());
        }
    }
}
