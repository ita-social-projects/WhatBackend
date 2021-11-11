using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage mentors and related data
    /// </summary>
    [Route("api/v{version:apiVersion}/mentors")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class MentorsController : ControllerBase
    {
        #region
        private readonly IMentorService _mentorService;
        private readonly ILessonService _lessonService;
        #endregion
        /// <summary>
        /// Mentors controller constructor
        /// </summary>
        public MentorsController(IMentorService mentorService, ILessonService lessonService)
        {
            _mentorService = mentorService;
            _lessonService = lessonService;
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
        public async Task<IList<LessonDto>> GetLessonsForMentor([FromBody] FilterLessonsRequestDto filterModel)
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
        [MapToApiVersion("2.0")]
        [HttpGet("{id}/groups")]
        public async Task<ActionResult<IList<MentorStudyGroupsDto>>> GetMentorStudyGroupsByMentorIdV2(long id)
        {
            var foundGroups = _mentorService
                    .CheckRoleAndIdMentor<IList<MentorStudyGroupsDto>>(id);

            if (foundGroups.Error == default)
            {
                foundGroups = Result<IList<MentorStudyGroupsDto>>.GetSuccess(await
                        _mentorService.GetMentorStudyGroupsByMentorIdAsync(id));
            }

            return foundGroups.ToActionResult();
        }

        /// <summary>
        /// Gets all of the mentor's study group
        /// </summary>
        /// <response code="200">Successful return of mentor's study groups</response>
        /// <response code="HTTP: 404, API: 3">Error, can not find mentor or mentor's study groups</response>
        [SwaggerResponse(200, type: typeof(IList<MentorStudyGroupsDto>))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [MapToApiVersion("1.0")]
        [HttpGet("{id}/groups")]
        public async Task<IList<MentorStudyGroupsDto>> GetMentorStudyGroupsByMentorId(long id)
        {
            var foundGroups = await _mentorService
                    .GetMentorStudyGroupsByMentorIdAsync(id);

            return foundGroups;
        }

        /// <summary>
        /// Gets all of the mentor's courses
        /// </summary>
        /// <response code="200">Successful return of mentor's courses</response>
        /// <response code="HTTP: 404, API: 3">Error, can not find mentor or mentor's courses</response>
        [SwaggerResponse(200, type: typeof(IList<MentorCoursesDto>))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [MapToApiVersion("2.0")]
        [HttpGet("{id}/courses")]
        public async Task<ActionResult<IList<MentorCoursesDto>>> GetMentorCoursesByMentorIdV2(long id)
        {
            var foundCourses = _mentorService
                    .CheckRoleAndIdMentor<IList<MentorCoursesDto>>(id);

            if (foundCourses.Error == default)
            {
                foundCourses = Result<IList<MentorCoursesDto>>.GetSuccess(await
                        _mentorService.GetMentorCoursesByMentorIdAsync(id));
            }

            return foundCourses.ToActionResult();
        }

        /// <summary>
        /// Gets all of the mentor's courses
        /// </summary>
        /// <response code="200">Successful return of mentor's courses</response>
        /// <response code="HTTP: 404, API: 3">Error, can not find mentor or mentor's courses</response>
        [SwaggerResponse(200, type: typeof(IList<MentorCoursesDto>))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [MapToApiVersion("1.0")]
        [HttpGet("{id}/courses")]
        public async Task<IList<MentorCoursesDto>> GetMentorCoursesByMentorId(long id)
        {
            var foundCourses = await _mentorService
                    .GetMentorCoursesByMentorIdAsync(id);

            return foundCourses;
        }

        /// <summary>
        /// Disable account by mentor id
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
        [MapToApiVersion("2.0")]
        [HttpGet("{id}/lessons")]
        public async Task<ActionResult<List<LessonDto>>> GetAllLessonsForMentorV2(long id)
        {
            var lessons = _mentorService
                    .CheckRoleAndIdMentor<IList<LessonDto>>(id);
       
            if (lessons.Error == default)
            {
                lessons = await _lessonService.GetAllLessonsForMentor(id);
            }

            return lessons.ToActionResult();
        }

        /// <summary>
        /// Returns list of lessons  for mentor
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Successful return of lessons list of given mentor</response>
        [SwaggerResponse(200, type: typeof(IList<LessonDto>))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [MapToApiVersion("1.0")]
        [HttpGet("{id}/lessons")]
        public async Task<ActionResult<List<LessonDto>>> GetAllLessonsForMentor(long id)
        {
            var lessons = await _lessonService.GetAllLessonsForMentor(id);

            return lessons.ToActionResult();
        }
    }
}
