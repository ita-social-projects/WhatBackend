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
        private readonly IExportService _exportService;

        /// <summary>
        /// Export controllers constructor
        /// </summary>
        public ExportController(IExportService exportService)
        {
            _exportService = exportService;
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
        [HttpPost]
        public async Task<IActionResult> GetStudentsClassbook([FromBody] StudentsRequestDto<ClassbookResultType> request)
        {
            var classbook = await _exportService.GetStudentsClassbook(request);

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
        [HttpPost]
        public async Task<IActionResult> GetStudentsResults([FromBody] StudentsRequestDto<StudentResultType> request)
        {
            var studentResults = await _exportService.GetStudentsResults(request);

            return File(await
                studentResults.GetByteArrayAsync(),
                studentResults.GetContentType(),
                studentResults.GetFileName());
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
        public async Task<IActionResult> GetStudentClassbook(long studentId, [FromBody] DashboardAnalyticsRequestDto<ClassbookResultType> request)
        {
            var studentClassbook = await _exportService.GetStudentClassbook(studentId, request);

            return File(await
                studentClassbook.GetByteArrayAsync(),
                studentClassbook.GetContentType(),
                studentClassbook.GetFileName());
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
        public async Task<IActionResult> GetStudentResults(long studentId, [FromBody] DashboardAnalyticsRequestDto<StudentResultType> request)
        {
            var studentResults = await _exportService.GetStudentResults(studentId, request);

            return File(await
                studentResults.GetByteArrayAsync(),
                studentResults.GetContentType(),
                studentResults.GetFileName());
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
        public async Task<IActionResult> GetStudentGroupResults(long courseId, [FromBody] DashboardAnalyticsRequestDto<StudentGroupResultType> request)
        {
            var studentGroupResults = await _exportService.GetStudentGroupResults(courseId, request);

            return File(await
                studentGroupResults.GetByteArrayAsync(),
                studentGroupResults.GetContentType(),
                studentGroupResults.GetFileName());
        }
    }
}
