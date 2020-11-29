using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core;
using CharlieBackend.Business.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to export aggregated data reports
    /// </summary>
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        #region
        private readonly IDashboardService _dashboardService;
        private readonly IStudentService _studentService;
        #endregion
        /// <summary>
        /// Dashboard controllers constructor
        /// </summary>
        public DashboardController(IDashboardService dashboardService, IStudentService studentService)
        {
            _dashboardService = dashboardService;
            _studentService = studentService;
        }

        /// <summary>
        /// Gets classbook results of every students lesson
        /// </summary>
        /// <param name="courceId">Gets classbook for every student of student groups in given cource</param>
        /// <param name="groupId">Gets classbook for students in given student group</param>
        /// <param name="request">In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of cource groups.
        /// "includeAnalytics": [] have to receive params for data to return ("StudentPresence", "StudentMarks" </param>
        /// <returns>Returns report of requested data</returns>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost("studentsClassbook/cource/{courceId}")]
        [HttpPost("studentsClassbook/studentgroup/{groupId}")]
        public async Task<ActionResult> GetStudentsClassbook(long courceId, long groupId, 
                [FromBody]StudentsClassbookRequestDto request)
        {
            var results = await _dashboardService
                .GetStudentsClassbookAsync(courceId, groupId, request);

            return results.ToActionResult();
        }

        /// <summary>
        /// Gets results of every student
        /// </summary>
        /// <param name="courceId">Gets report for student groups in given cource</param>
        /// <param name="groupId">Gets report for students in given student group</param>
        /// <param name="request">In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of cource groups.
        /// "includeAnalytics": [] have to receive params for data to return ("AverageStudentMark", "AverageStudentVisits" </param>
        /// <returns>Returns report of requested data</returns>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost("studentsResults/cource/{courceId}")]
        [HttpPost("studentsResults/studentgroup/{groupId}")]
        public async Task<ActionResult> GetStudentsResults(long courceId, long groupId, 
                [FromBody]StudentsResultsRequestDto request)
        {
            var results = await _dashboardService
                .GetStudentsResultAsync(courceId, groupId, request);

            return results.ToActionResult();
        }

        /// <summary>
        /// Gets classbook data of given student
        /// </summary>
        /// <param name="studentId">Student id</param>
        /// <param name="request">In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of students group.
        /// "includeAnalytics": [] have to receive params for data to return "StudentPresence", "StudentMarks" </param>
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpPost("studentClassbook/{studentId}")]
        public async Task<ActionResult> GetStudentClassbook(long studentId, [FromBody]StudentsClassbookRequestDto request)
        {
            string authHeader = Request.Headers["Authorization"];

            var results = await _dashboardService
            .GetStudentClassbookAsync(studentId, request, authHeader);

            return results.ToActionResult();
        }

        /// <summary>
        /// Gets results data of given student
        /// </summary>
        /// <param name="studentId">Student id</param>
        /// <param name="request">In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of students group.
        /// "includeAnalytics": [] have to receive params for data to return "AverageStudentMark", "AverageStudentVisits" </param>
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpPost("studentResults/{studentId}")]
        public async Task<ActionResult> GetStudentResults(long studentId, [FromBody]StudentsResultsRequestDto request)
        {
            string authHeader = Request.Headers["Authorization"];

            var results = await _dashboardService
            .GetStudentResultAsync(studentId, request, authHeader);

            return results.ToActionResult();
        }

        /// <summary>
        /// Gets report data of student group results
        /// </summary>
        /// <param name="courceId">Cource id</param>
        /// <param name="request">In body you can mention: "startDate", "finishtDate" is optional param to filter 
        /// learning period of students group.
        /// "includeAnalytics": [] have to receive params for data to return "AverageStudentGroupMark", "AverageStudentGroupVisitsPercentage" </param>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost("studentGroupResults/{courceId}")]
        public async Task<ActionResult> GetStudentGroupResults(long courceId, [FromBody]StudentGroupsResultsRequestDto request)
        {
            var results = await _dashboardService
            .GetStudentGroupResultAsync(courceId, request);

            return results.ToActionResult();
        }


    }
}