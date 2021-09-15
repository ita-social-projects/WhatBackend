using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.Export;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        private readonly IExportServiceContext _exportService;//TODO Maybe strategy

        /// <summary>
        /// Export controllers constructor
        /// </summary>
        public ExportController(IExportServiceContext exportService,
                                IDashboardService dashboardService)
        {
            _exportService = exportService;
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
            var results = await _dashboardService
                .GetStudentsClassbookAsync(request.GetStudentsRequestDto());

            var classbook = await _exportService.GetStudentsClassbook(results.Data);

            if (classbook.Error == null)
            {
                return File(classbook.Data.ByteArray,
                           classbook.Data.ContentType,
                           classbook.Data.Filename);
            }

            return classbook.ToActionResult();
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
            var results = await _dashboardService
                .GetStudentsResultAsync(request.GetStudentsRequestDto());

            var studentResults = await _exportService.GetStudentsResults(results.Data);

            if (studentResults.Error == null)
            {
                return File(studentResults.Data.ByteArray,
                           studentResults.Data.ContentType,
                           studentResults.Data.Filename);
            }

            return studentResults.ToActionResult();
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
            var results = await _dashboardService
                .GetStudentClassbookAsync(studentId, request.GetDashboardAnalyticsRequestDto());

            var studentClassbook = await _exportService.GetStudentClassbook(results.Data);

            if (studentClassbook.Error == null)
            {
                return File(studentClassbook.Data.ByteArray,
                           studentClassbook.Data.ContentType,
                           studentClassbook.Data.Filename);
            }

            return studentClassbook.ToActionResult();
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
            var results = await _dashboardService
                .GetStudentResultAsync(studentId, request.GetDashboardAnalyticsRequestDto());

            var studentResults = await _exportService.GetStudentResults(results.Data);

            if (studentResults.Error == null)
            {
                return File(studentResults.Data.ByteArray,
                           studentResults.Data.ContentType,
                           studentResults.Data.Filename);
            }

            return studentResults.ToActionResult();
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
            var results = await _dashboardService
            .GetStudentGroupResultAsync(courseId, request.GetDashboardAnalyticsRequestDto());

            var studentGroupResults = await _exportService.GetStudentGroupResults(results.Data);


            if (studentGroupResults.Error == null)
            {
                return File(studentGroupResults.Data.ByteArray,
                           studentGroupResults.Data.ContentType,
                           studentGroupResults.Data.Filename);
            }

            return studentGroupResults.ToActionResult();
        }

        /// <summary>
        /// Gets a csv file with a list of students by group number
        /// </summary>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet("studentsOfGroup/{groupId}/{extension}")]
        public async Task<IActionResult> GetStudentsOfGroupList(long groupId, FileExtension extension)
        {
            if (!_exportService.SetServise(extension))
            {
                return Result<FileDto>.GetError(ErrorCode.ValidationError,
                        "Extension wasn't chosen").ToActionResult();     
            }

            var resultStudentList = await _exportService.GetListofStudentsByGroupId(groupId);

            if (resultStudentList.Error == null)
            {
                return File(resultStudentList.Data.ByteArray,
                           resultStudentList.Data.ContentType,
                           resultStudentList.Data.Filename);
            }

            return resultStudentList.ToActionResult();
        }
    }
}
