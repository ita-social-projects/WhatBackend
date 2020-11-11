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
        /// Gets data and returns report for given report parameters. Parameters sent in body
        /// </summary>
        /// <param name="request">is student group id, "courceId" is id or student course, 
        /// "reportData": [] have to receive data type (eg. "averageStudentMark", "averageStudentVisits"
        /// , "classbook"  to return</param>
        /// <returns>Returns report of requested data</returns>
        [Authorize(Roles = "2, 3, 4")]
        [HttpPost]
        public async Task<ActionResult> GetResults(DashboardRequestDto request)
        {
            var results = await _dashboardService
                .GetResultAsync(request);

            return results.ToActionResult();
        }
    }
}