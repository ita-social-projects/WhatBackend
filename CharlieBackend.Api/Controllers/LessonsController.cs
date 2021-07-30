﻿ using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Lesson;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using CharlieBackend.Core;
using Newtonsoft.Json.Linq;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage lessons
    /// </summary>
    [Route("api/lessons")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        /// <summary>
        /// Lessonscontroller's constructor
        /// </summary>
        /// <param name="lessonService"></param>
        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        /// <summary>
        /// Adds new lesson
        /// </summary>
        /// <remarks>
        /// In body of request **"id"** is not recessary
        /// </remarks>
        /// <response code="200">Successful addition of the lesson</response>
        /// <response code="HTTP: 422, API: ">Can not create lesson</response>
        [SwaggerResponse(200, type: typeof(LessonDto))]
        [Authorize(Roles = "Mentor, Admin")]
        [HttpPost]
        public async Task<ActionResult> PostLesson(CreateLessonDto lessonDto)
        {
            var createdLesson = await _lessonService.CreateLessonAsync(lessonDto);          
            if (createdLesson == null)
            {
                return StatusCode(422, "Cannot create lesson");
            }

            return createdLesson.ToActionResult();
        }

        /// <summary>
        /// Assinging mentor to lesson
        /// </summary>
        /// <response code="200">Successful assinging mentor to lesson</response>
        /// <response code="HTTP: 404, API: 3">Error, lesson does not exist</response>
        /// <response code="HTTP: 404, API: 3">Error, mentor is not found</response>
        [SwaggerResponse(200, type: typeof(Lesson))]
        [Authorize(Roles = "Admin, Secretary")]
        [Route("assign")]
        [HttpPost]
        public async Task<ActionResult> AssignMentorToLesson(AssignMentorToLessonDto ids)
        {
            var changedLesson = await _lessonService.AssignMentorToLessonAsync(ids);

            return changedLesson.ToActionResult();
        }

        /// <summary>
        /// Gets list of all lessons
        /// </summary>
        /// <response code="200">Successful return of lessons list</response>
        [SwaggerResponse(200, type: typeof(List<LessonDto>))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet]
        public async Task<ActionResult<List<LessonDto>>> GetAllLessons()
        {
            var lessons = await _lessonService.GetAllLessonsAsync();
            
            return lessons.ToActionResult();
        }

        /// <summary>
        /// Updates given lesson
        /// </summary>
        /// <remarks>
        /// In body of request **id** is not recessary
        /// </remarks>
        /// <response code="200">Successful update of given lesson</response>
        /// <response code="HTTP: 409, API: 5">Can not update lesson</response>
        [SwaggerResponse(200, type: typeof(LessonDto))]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutLesson(long id, UpdateLessonDto lessonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedLesson = await _lessonService.UpdateLessonAsync(id, lessonDto);

            if (updatedLesson != null)
            {
                return updatedLesson.ToActionResult();
            }

            return StatusCode(409, "Cannot update.");
        }

        /// <summary>
        /// Get lesson information by lesson id
        /// </summary>
        /// <response code="200">Successful return of lesson</response>
        /// <response code="404">Error, can not find lesson</response>
        [SwaggerResponse(200, type: typeof(LessonDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetLessonById(long id)
        {
            var lessonModelResult = await _lessonService.GetLessonByIdAsync(id);

            return lessonModelResult.ToActionResult();
        }

        /// <summary>
        /// Check if lesson was done
        /// </summary>
        /// <response code="200">Successful request</response>
        /// <response code="404">Lesson not found</response>
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet("{id}/isdone")]
        public async Task<ActionResult<JObject>> IsLessonDone(long id)
        {
            dynamic result = new JObject();
            result.isDone = (await _lessonService.IsLessonDoneAsync(id)).Data;

            return result;
        }
    }
}
