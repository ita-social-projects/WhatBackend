using System;
using System.Linq;
using CharlieBackend.Core;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Dashboard;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to export aggregated data reports
    /// </summary>
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        /// <summary>
        /// Dashboard controllers constructor
        /// </summary>
        public DashboardController(IDashboardService dashboardService)
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
        [SwaggerResponse(200, type: typeof(StudentsClassbookResultDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost("studentsClassbook")]
        public async Task<ActionResult> GetStudentsClassbook([FromBody]StudentsRequestDto<ClassbookResultType> request)
        {
            var results = await _dashboardService
                .GetStudentsClassbookAsync(request);

            return results.ToActionResult();
        }

        /// <summary>
        /// Gets results of every student
        /// </summary>
        /// <param name="request">
        /// 1. Mention "courseId" or "studentGroupId" to filter all cource groups or exact student group.
        /// 2. In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of cource groups.
        /// 3. "includeAnalytics": ["AverageStudentMark", "AverageStudentVisits"] have to receive params for result to return</param>
        [SwaggerResponse(200, type: typeof(StudentsResultsDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost("studentsResults")]
        public async Task<ActionResult> GetStudentsResults([FromBody]StudentsRequestDto<StudentResultType> request)
        {
            var results = await _dashboardService
                .GetStudentsResultAsync(request);

            return results.ToActionResult();
        }

        /// <summary>
        /// Gets classbook data of given student
        /// </summary>
        /// <param name="studentId">Student id</param>
        /// <param name="request">In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of students group.
        /// "includeAnalytics": ["StudentPresence", "StudentMarks"] options which report type to receive</param>
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpPost("studentClassbook/{studentId}")]
        public async Task<ActionResult> GetStudentClassbook(long studentId, [FromBody]DashboardAnalyticsRequestDto<ClassbookResultType> request)
        {
            var userContext = HttpContext.User;

            var results = await _dashboardService
            .GetStudentClassbookAsync(studentId, request, userContext);

            return results.ToActionResult();
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
        public async Task<ActionResult> GetStudentResults(long studentId, [FromBody]DashboardAnalyticsRequestDto<StudentResultType> request)
        {
            var userContext = HttpContext.User;

            var results = await _dashboardService
            .GetStudentResultAsync(studentId, request, userContext);

            return results.ToActionResult();
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
        public async Task<ActionResult> GetStudentGroupResults(long courseId, [FromBody]DashboardAnalyticsRequestDto<StudentGroupResultType> request)
        {
            var results = await _dashboardService
            .GetStudentGroupResultAsync(courseId, request);

            return results.ToActionResult();
        }
    }
}