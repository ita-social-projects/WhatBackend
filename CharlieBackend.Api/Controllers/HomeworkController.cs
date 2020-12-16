using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Homework;
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
        [HttpPost("addHometask")]
        public async Task<ActionResult> PostHometask([FromBody]CreateHometaskDto request)
        {
            var results = await _homeworkService
                .CreateHometaskAsync(request);

            return results.ToActionResult();
        }

        /// <summary>
        /// Gets all hometasks of course
        /// </summary>
        [Authorize(Roles = "Admin, Mentor")]
        [HttpGet("getHometaskOfCourse/{courseId}")]
        public async Task<ActionResult> GetHometaskOfCourse(long courseId)
        {
            var results = await _homeworkService
                .GetHometaskOfCourseAsync(courseId);

            return results.ToActionResult();
        }

        /// <summary>
        /// Gets hometask by id
        /// </summary>
        [Authorize(Roles = "Admin, Mentor, Student")]
        [HttpGet("getHometask/{hometaskId}")]
        public async Task<ActionResult> GetHometaskById(long hometaskId)
        {
            var results = await _homeworkService
                .GetHometaskByIdAsync(hometaskId);

            return results.ToActionResult();
        }
    }
}