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

namespace CharlieBackend.Api.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        #region
        private readonly IDashboardService _dashboardService;
        #endregion

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Gets data and returns report for given report parameters. Report of students results in student groups.
        /// Parameters sent in body
        /// </summary>
        /// <param name="request">You have to mention of of: "groupId" is student group id, "courceId" is id of student course.
        /// "startDate" is optional param to filter start date of group start.
        /// "includeAnalytics": [] have to receive params for data to return ("AverageStudentMark", "AverageStudentVisits", 
        /// "StudentPresence", "StudentMarks" </param>
        /// <returns>Returns report of requested data</returns>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost]
        public async Task<ActionResult> GetStudentsResults(DashboardRequestDto request)
        {
            var results = await _dashboardService
                .GetStudentsResultAsync(request);

            return results.ToActionResult();
        }
    }
}