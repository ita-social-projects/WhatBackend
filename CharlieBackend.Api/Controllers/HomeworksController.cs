﻿using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.DTO.Visit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to make operations with homework
    /// </summary>
    [Route("api/v{version:apiVersion}/homeworks")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class HomeworksController : ControllerBase
    {
        private readonly IHomeworkService _homeworkService;

        /// <summary>
        /// Homework controllers constructor
        /// </summary>
        public HomeworksController(IHomeworkService homeworkService)
        {
            _homeworkService = homeworkService;
        }

        /// <summary>
        /// Get all homework tasks with ThemeName
        /// </summary>
        /// <returns>
        /// All Homework's entities
        /// </returns>
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<IList<HomeworkDto>>> GetHomeworks()
        {
            var homeworks = await _homeworkService.GetHomeworks();
            return homeworks.ToActionResult();
        }

        /// <summary>
        /// Get homework task by id
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkDto))]
        [Authorize(Roles = "Admin, Mentor, Student")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetHomeworkById(long id)
        {
            var results = await _homeworkService
                        .GetHomeworkByIdAsync(id);

            return results.ToActionResult();
        }

        /// <summary>
        /// Get homework tasks not done
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet("notdonehomework/{studentGroupId}/{studentId}")]
        public async Task<ActionResult> GetHomeworkNotDone(long studentGroupId, long studentId, DateTime? dueDate = null)
        {
            var results = await _homeworkService
                        .GetHomeworkNotDone(studentGroupId, studentId, dueDate);

            return results.ToActionResult();
        }

        /// <summary>
        /// Add new homework task
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkDto))]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpPost]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> PostHomework([FromBody] HomeworkRequestDto request)
        {
            var results = await _homeworkService
                        .CreateHomeworkAsync(request);

            return results.ToActionResult();
        }

        /// <summary>
        /// Get homework tasks by group id, theme id or course id
        /// </summary>
        /// <param name="request">
        /// 1. Mention "groupId" or "courseId" or "themeId" to get homeworks of a specific group, course, theme.
        /// 2. You can mention optional parameters — "startDate", "finishtDate" to get homeworks depending on publishing date of homework
        /// </param>
        [SwaggerResponse(200, type: typeof(List<HomeworkDto>))]
        [Authorize(Roles = "Mentor, Admin, Secretary")]
        [HttpPost("getHomeworks")]
        public async Task<ActionResult> GetHomeworks([FromBody] GetHomeworkRequestDto request)
        {
            var results = await _homeworkService
                        .GetHomeworksAsync(request);

            return results.ToActionResult();
        }

        /// <summary>
        /// Update homework task
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkDto))]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpPut("{id}")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> PutHomework(long id, [FromBody] HomeworkRequestDto updateHomeworkDto)
        {
            var results = await _homeworkService
                        .UpdateHomeworkAsync(id, updateHomeworkDto);

            return results.ToActionResult();
        }

        /// <summary>
        /// Update student mark
        /// </summary>
        /// <response code="200">Successful updating of the mark</response>
        /// <response code="HTTP: 404">Student homework not found</response>
        [SwaggerResponse(200, type: typeof(VisitDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [MapToApiVersion("1.0")]
        [HttpPut("updatemark")]
        public async Task<ActionResult> UpdateMark([FromBody] UpdateMarkRequestDto request)
        {
            return (await _homeworkService.UpdateMarkAsync(request)).ToActionResult();
        }
    }
}
