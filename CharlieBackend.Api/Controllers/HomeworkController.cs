using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to make operations with homework
    /// </summary>
    [Route("api/homework")]
    [ApiController]
    public class HomeworkController : ControllerBase
    {
        private readonly IHomeworkService _homeworkService;

        /// <summary>
        /// Homework controllers constructor
        /// </summary>
        public HomeworkController(IHomeworkService homeworkService)
        {
            _homeworkService = homeworkService;
        }

        /// <summary>
        /// Adds hometask
        /// </summary>
        [Authorize(Roles = "Admin, Mentor")]
        [HttpPost("studentsClassbook")]
        public async Task<ActionResult> PostHometask([FromBody]CreateHometaskDto request)
        {
            var results = await _homeworkService
                .PostHometask(request);

            return results.ToActionResult();
        }
    }
}