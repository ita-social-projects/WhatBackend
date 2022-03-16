using CharlieBackend.Business.Services.FileServices.ExportFileServices;
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
    [Route("api/v{version:apiVersion}/exports")]
    [ApiVersion("2.0")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IExportServiceProvider _exportProvider;

        /// <summary>
        /// Export controllers constructor
        /// </summary>
        public ExportController(IExportServiceProvider exportService,
                                IDashboardService dashboardService)
        {
            _exportProvider = exportService;
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Gets classbook data of every students lesson
        /// </summary>
        /// <param name="request">
        /// Mention "courseId" or "studentGroupId" to filter all course groups or exact student group.
        /// In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of course groups.
        /// "includeAnalytics": ["StudentPresence", "StudentMarks"] params to choose what to return:
        ///                                                            0 - presence
        ///                                                            1 - marks
        ///                                                            0, 1 - both presence and marks</param>
        /// <param name="extension"> Extension of file that you want to get:
        ///                                                            0 - HTML
        ///                                                            1 - XLSX
        ///                                                            2 - CSV </param>
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("studentsClassbook/{extension}")]
        [HttpPost]
        public async Task<IActionResult> GetStudentsClassbook(FileExtension extension, 
                [FromBody] StudentsRequestDto<ClassbookResultType>request)
        {
            var exportService = _exportProvider.GetExportService(extension);

            if (exportService == null)
            {
                return Result<FileDto>.GetError(ErrorCode.ValidationError,
                        "Extension wasn't chosen").ToActionResult();
            }

            var results = await _dashboardService.GetStudentsClassbookAsync(request);

            var classbook = await exportService.GetStudentsClassbook(results.Data);

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
        /// Mention "courseId" or "studentGroupId" to filter all course groups or exact student group.
        /// In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of course groups.
        /// "includeAnalytics": ["AverageStudentMark", "AverageStudentVisits"] have to receive params for result to return:
        ///                                                            0 - presence
        ///                                                            1 - marks
        ///                                                            0, 1 - both presence and marks</param>
        /// <param name="extension"> Extension of file that you want to get:
        ///                                                            0 - HTML
        ///                                                            1 - XLSX
        ///                                                            2 - CSV</param>
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("studentsResults/{extension}")]
        [HttpPost]
        public async Task<IActionResult> GetStudentsResults(FileExtension extension, 
                [FromBody] StudentsRequestDto<StudentResultType> request)
        {
            var exportService = _exportProvider.GetExportService(extension);

            if (exportService == null)
            {
                return Result<FileDto>.GetError(ErrorCode.ValidationError,
                        "Extension wasn't chosen").ToActionResult();
            }

            var results = await _dashboardService.GetStudentsResultAsync(request);

            var studentResults = await exportService.GetStudentsResults(results.Data);

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
        /// "includeAnalytics": ["StudentPresence", "StudentMarks"] params to choose what to return:
        ///                                                            0 - presence
        ///                                                            1 - marks
        ///                                                            0, 1 - both presence and marks</param>
        /// <param name="extension"> Extension of file that you want to get:
        ///                                                            0 - HTML
        ///                                                            1 - XLSX
        ///                                                            2 - CSV </param>
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("studentClassbook/{studentId}/{extension}")]
        [HttpPost]
        public async Task<IActionResult> GetStudentClassbook(long studentId, FileExtension extension,
                [FromBody] DashboardAnalyticsRequestDto<ClassbookResultType> request)
        {
            var exportService = _exportProvider.GetExportService(extension);

            if (exportService == null)
            {
                return Result<FileDto>.GetError(ErrorCode.ValidationError,
                        "Extension wasn't chosen").ToActionResult();
            }

            var results = await _dashboardService.GetStudentClassbookAsync(studentId, request);

            var studentClassbook = await exportService.GetStudentClassbook(results.Data);

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
        /// "includeAnalytics": ["AverageStudentMark", "AverageStudentVisits"] have to receive params for data to return:
        ///                                                            0 - presence
        ///                                                            1 - marks
        ///                                                            0, 1 - both presence and marks</param>
        /// <param name="extension"> Extension of file that you want to get:
        ///                                                            0 - HTML
        ///                                                            1 - XLSX
        ///                                                            2 - CSV </param>
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpPost("studentResults/{studentId}/{extension}")]
        public async Task<IActionResult> GetStudentResults(long studentId, FileExtension extension,
                [FromBody] DashboardAnalyticsRequestDto<StudentResultType> request)
        {
            var exportService = _exportProvider.GetExportService(extension);

            if (exportService == null)
            {
                return Result<FileDto>.GetError(ErrorCode.ValidationError,
                        "Extension wasn't chosen").ToActionResult();
            }

            var results = await _dashboardService
                .GetStudentResultAsync(studentId, request);

            var studentResults = await exportService.GetStudentResults(results.Data);

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
        /// "includeAnalytics": ["AverageStudentGroupMark", "AverageStudentGroupVisitsPercentage"] have to receive params for data to return:
        ///                                                            0 - presence
        ///                                                            1 - marks
        ///                                                            0, 1 - both presence and marks</param>
        /// <param name="extension"> Extension of file that you want to get:
        ///                                                            0 - HTML
        ///                                                            1 - XLSX
        ///                                                            2 - CSV </param>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost("studentGroupResults/{courseId}/{extension}")]
        public async Task<IActionResult> GetStudentGroupResults(long courseId, FileExtension extension,
                [FromBody] DashboardAnalyticsRequestDto<StudentGroupResultType> request)
        {
            var exportService = _exportProvider.GetExportService(extension);

            if (exportService == null)
            {
                return Result<FileDto>.GetError(ErrorCode.ValidationError,
                        "Extension wasn't chosen").ToActionResult();
            }

            var results = await _dashboardService
                .GetStudentGroupResultAsync(courseId, request);

            var studentGroupResults = await exportService
                .GetStudentGroupResults(results.Data);


            if (studentGroupResults.Error == null)
            {
                return File(studentGroupResults.Data.ByteArray,
                           studentGroupResults.Data.ContentType,
                           studentGroupResults.Data.Filename);
            }

            return studentGroupResults.ToActionResult();
        }

        /// <summary>
        /// Gets a file with a list of students by group number
        /// </summary>
        /// <param name="groupId"> Group id of students that list you want to get </param>
        /// <param name="extension"> Extension of file that you want to get:
        ///                                                            0 - HTML
        ///                                                            1 - XLSX
        ///                                                            2 - CSV </param>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet("studentsOfGroup/{groupId}/{extension}")]
        public async Task<IActionResult> GetStudentsOfGroupList(long groupId, FileExtension extension)
        {
            var exportService = _exportProvider.GetExportService(extension);

            if (exportService == null)
            {
                return Result<FileDto>.GetError(ErrorCode.ValidationError,
                        "Extension wasn't chosen").ToActionResult();
            }

            var resultStudentList = await exportService.GetListofStudentsByGroupId(groupId);

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
