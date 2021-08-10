﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Mentor;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Core.DTO.Homework;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage mentors and related data
    /// </summary>
    [Route("api/mentors")]
    [ApiController]
    public class MentorsController : ControllerBase
    {
        #region
        private readonly IMentorService _mentorService;
        private readonly ILessonService _lessonService;
        private readonly IHomeworkService _homeworkService;
        #endregion
        /// <summary>
        /// Mentors controller constructor
        /// </summary>
        public MentorsController(IMentorService mentorService,
                ILessonService lessonService,
                IHomeworkService homeworkService)
        {
            _mentorService = mentorService;
            _lessonService = lessonService;
            _homeworkService = homeworkService;
        }

        /// <summary>
        /// Assign account to mentor 
        /// </summary>
        /// <response code="200">Successful assigning of account to mentor </response>
        /// <response code="HTTP: 404, API: 3">Can not find account</response>
        /// <response code="HTTP: 400, API: 0">Error, account already assigned</response>
        [SwaggerResponse(200, type: typeof(MentorDto))]
        [Authorize(Roles = "Admin, Secretary")]
        [HttpPost("{accountId}")]
        public async Task<ActionResult> PostMentor(long accountId)
        {
            var createdMentorModel = await _mentorService.CreateMentorAsync(accountId);

            return createdMentorModel.ToActionResult();
        }

        /// <summary>
        /// Get filter list of lessons for mentor
        /// </summary>
        /// <response code="200">Returned filtered list of lessons for mentor </response>
        [SwaggerResponse(200, type: typeof(IList<LessonDto>))]
        [Authorize(Roles = "Mentor")]
        [HttpPost("lessons")]
        public async Task<IList<LessonDto>> GetLessonsForMentor([FromBody]FilterLessonsRequestDto filterModel)
        {
            var lessons = await _lessonService.GetLessonsForMentorAsync(filterModel);

            return lessons;
        }

        /// <summary>
        /// Get only active mentors
        /// </summary>
        /// <response code="200">Successful return of mentors list</response>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet("active")]
        public async Task<IList<MentorDetailsDto>> GetAllActiveMentors()
        {
            var mentors = await _mentorService.GetAllActiveMentorsAsync();

            return mentors;
        }
        
        /// <summary>
        /// Get mentor information by mentor id
        /// </summary>
        /// <response code="200">Successful return of mentor</response>
        /// <response code="404">Error, can not find mentor</response>
        [SwaggerResponse(200, type: typeof(MentorDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MentorDto>> GetMentorById(long id)
        {
            var mentorModelResult = await _mentorService.GetMentorByIdAsync(id);

            if (mentorModelResult != null)
            {
                return mentorModelResult.ToActionResult();
            }
            return NotFound("Cannot find mentor with such id.");
        }

        /// <summary>
        /// Gets list of all mentors
        /// </summary>
        /// <response code="200">Successful return of mentors list</response>
        [SwaggerResponse(200, type: typeof(IList<MentorDto>))]
        [Authorize(Roles = "Admin, Secretary")]
        [HttpGet]
        public async Task<ActionResult<IList<MentorDetailsDto>>> GetAllMentors()
        {
            var mentorsModels = await _mentorService.GetAllMentorsAsync();

            return Ok(mentorsModels);
        }

        /// <summary>
        /// Update of mentor
        /// </summary>
        /// <remarks>
        /// **courseIds** and **studentGroupIds** is optional
        /// </remarks>
        /// <response code="200">Successful update of mentor</response>
        /// <response code="HTTP: 404, API: 3">Mentor not found</response>
        /// <response code="HTTP: 400, API: 5">Can not update mentor due to data conflict</response>
        [SwaggerResponse(200, type: typeof(MentorDto))]
        [Authorize(Roles = "Admin, Secretary")]
        [HttpPut("{mentorId}")]
        public async Task<ActionResult> PutMentor(long mentorId, [FromBody] UpdateMentorDto mentorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedMentor = await _mentorService.UpdateMentorAsync(mentorId, mentorModel);

            return updatedMentor.ToActionResult();
        }

        /// <summary>
        /// Gets all of the mentor's study group
        /// </summary>
        /// <response code="200">Successful return of mentor's study groups</response>
        /// <response code="HTTP: 404, API: 3">Error, can not find mentor or mentor's study groups</response>
        [SwaggerResponse(200, type: typeof(IList<MentorStudyGroupsDto>))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpGet("{id}/groups")]
        public async Task<ActionResult<IList<MentorStudyGroupsDto>>> GetMentorStudyGroupsByMentorId(long id)
        {
            var foundGroups = await _mentorService.CheckRoleAndIdMentor(id,
                    new Result<IList<MentorStudyGroupsDto>>());

            if (foundGroups.Error == default)
            {
                foundGroups = Result<IList<MentorStudyGroupsDto>>.GetSuccess(await
                        _mentorService.GetMentorStudyGroupsByMentorIdAsync(id));
            }
            
            return foundGroups.ToActionResult();
        }

        /// <summary>
        /// Gets all of the mentor's courses
        /// </summary>
        /// <response code="200">Successful return of mentor's courses</response>
        /// <response code="HTTP: 404, API: 3">Error, can not find mentor or mentor's courses</response>
        [SwaggerResponse(200, type: typeof(IList<MentorCoursesDto>))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpGet("{id}/courses")]
        public async Task<ActionResult<IList<MentorCoursesDto>>> GetMentorCoursesByMentorId(long id)
        {
            var foundCourses = await _mentorService.CheckRoleAndIdMentor(id,
                    new Result<IList<MentorCoursesDto>>());

            if (foundCourses.Error == default)
            {
                foundCourses = Result<IList<MentorCoursesDto>>.GetSuccess(await
                        _mentorService.GetMentorCoursesByMentorIdAsync(id));
            }

            return foundCourses.ToActionResult();
        }

        /// <summary>
        /// Disable mentor's account
        /// </summary>
        /// <response code="204">Mentor's account successfully disabled</response>
        /// <response code="HTTP: 400, API: 3">Mentor not found</response>
        /// <response code="HTTP: 409, API: 5">Mentor's account is already disabled</response>
        [Authorize(Roles = "Admin, Secretary")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DisableMentor(long id)
        {
            var disabledMentorModel = await _mentorService.DisableMentorAsync(id);

            return disabledMentorModel.ToActionResult();
        }

        /// <summary>
        /// Enable mentor's account
        /// </summary>
        /// <response code="204">Mentor's account successfully enabled</response>
        /// <response code="HTTP: 400, API: 3">Mentor not found</response>
        /// <response code="HTTP: 409, API: 5">Mentor's account is already enabled</response>
        [Authorize(Roles = "Admin, Secretary")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<bool>> EnableMentor(long id)
        {
            var disabledMentorModel = await _mentorService.EnableMentorAsync(id);

            return disabledMentorModel.ToActionResult();
        }

        /// <summary>
        /// Returns list of lessons  for mentor
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Successful return of lessons list of given mentor</response>
        [SwaggerResponse(200, type: typeof(IList<LessonDto>))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet("{id}/lessons")]
        public async Task<ActionResult<List<LessonDto>>> GetAllLessonsForMentor(long id)
        {
            var lessons = await _mentorService.CheckRoleAndIdMentor(id, new Result<IList<LessonDto>>());

            if (lessons.Error == default)
            {
                lessons = await _lessonService.GetAllLessonsForMentor(id);
            }

            return lessons.ToActionResult();
        }

        /// <summary>
        /// Returns list of filtered homeworks of mentor 
        /// </summary>
        /// <param name="filter">filter for request to db</param>
        /// <param name="mentorId">ID of the mentor whose homework we
        /// are looking for</param>
        /// <response code="200">Successful return of homeworks list of
        /// given mentor</response>
        ///<response code="401">Mentor can get only his information</response>
        ///<response code="404">Not found account of mentor</response>
        [SwaggerResponse(200, type: typeof(IList<HomeworkDto>))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet("{mentorId}/homeworks")]
        public async Task<ActionResult<List<HomeworkDto>>> GetMentorHomeworks(
                [FromBody]HomeworkFilterDto filter, long mentorId) 
        {
            var homeworks = await _mentorService.CheckRoleAndIdMentor(mentorId,
                    new Result<IList<HomeworkDto>>());

            if (homeworks.Error == default)
            {
                homeworks = await _homeworkService.GetMentorFilteredHW(
                        mentorId, filter);
            }

            return homeworks.ToActionResult();
        }
    }
}
