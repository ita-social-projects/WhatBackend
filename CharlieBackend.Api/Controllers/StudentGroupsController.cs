using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core;
using CharlieBackend.Core.Models.ResultModel;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Student groups controller
    /// </summary>
    [Route("api/student_groups")]
    [ApiController]
    public class StudentGroupsController : ControllerBase
    {

        private readonly IStudentGroupService _studentGroupService;

        /// <summary>
        /// Student Groups controllers constructor
        /// </summary>
        public StudentGroupsController(IStudentGroupService studentGroupService)
        {
            _studentGroupService = studentGroupService;
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteStudentGroup(long id)
        //{
        //    var x = await _studentGroupService.SearchStudentGroup(id);
        //    if (x == null)
        //        return Ok("Not Found");
        //    else
        //    {
        //        _studentGroupService.DeleteStudentGrop(id);
        //        return Ok("Done");
        //    }
        //}

        /// <summary>
        /// Adding of new student group
        /// </summary>
        /// <response code="200">Successful group addition</response>
        /// <response code="HTTP: 422, API: 4">Error, given group data already exist</response>
        /// <response code="HTTP: 400, API: 0">Error, given group data unprocessable</response>
        [SwaggerResponse(200, type: typeof(StudentGroupDto))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpPost]
        public async Task<ActionResult<StudentGroupDto>> PostStudentGroup([FromBody]CreateStudentGroupDto studentGroup)
        {
            
            var isStudentGroupNameExist = await _studentGroupService
                    .IsGroupNameExistAsync(studentGroup.Name);

            if (isStudentGroupNameExist.Data)
            {
                return Result<StudentGroupDto>.GetError(ErrorCode.UnprocessableEntity, "Group name already exists").ToActionResult();
            }

            var resStudentGroup = await _studentGroupService.CreateStudentGroupAsync(studentGroup);
          
            return resStudentGroup.ToActionResult();
        }

        /// <summary>
        /// Updates given student group
        /// </summary>
        /// <response code="200">Successful update of student group</response>
        /// <response code="HTTP: 422, API: 4">Error, given group name already exist</response>
        /// <response code="HTTP: 400, API: 0">Error, given group data is wrong</response>
        [SwaggerResponse(200, type: typeof(UpdateStudentGroupDto))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateStudentGroupDto>> PutStudentGroup(long id, [FromBody]UpdateStudentGroupDto studentGroupDto)
        {
            var isStudentGroupNameExist = await _studentGroupService
                   .IsGroupNameExistAsync(studentGroupDto.Name);

            if (isStudentGroupNameExist.Data)
            {
                return Result<StudentGroupDto>.GetError(ErrorCode.UnprocessableEntity, "Group name already exists").ToActionResult();
            }

            var updatedStudentGroup = await _studentGroupService
                    .UpdateStudentGroupAsync(id, studentGroupDto);

            return updatedStudentGroup.ToActionResult();
        }

        /// <summary>
        /// Update students of stunde group
        /// </summary>
        /// <response code="200">Successful update of student group students</response>
        /// <response code="HTTP: 400, API: 0">Error, given data is wrong</response>
        [SwaggerResponse(200, type: typeof(UpdateStudentsForStudentGroup))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpPut("{id}/students")]
        public async Task<ActionResult<UpdateStudentsForStudentGroup>> PutStudentsOfStudentGroup(long id, [FromBody]UpdateStudentsForStudentGroup studentGroupDto) 
        {
            var updatedStudentGroup = await _studentGroupService
                    .UpdateStudentsForStudentGroupAsync(id, studentGroupDto);

            return updatedStudentGroup.ToActionResult();
        }
        
        /// <summary>
        /// Gets all student groups
        /// </summary>
        /// <response code="200">Successful return of students of student group</response>
        [SwaggerResponse(200, type: typeof(IList<StudentGroupDto>))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpGet]
        public async Task<ActionResult<List<StudentGroupDto>>> GetAllStudentGroups()
        {
            return Ok(await _studentGroupService.GetAllStudentGroupsAsync());
        }

        /// <summary>
        /// Gets student group
        /// </summary>
        /// <response code="200">Successful return of student group</response>
        /// <response code="HTTP: 404, API: 3">Error, can not find student group</response>
        [SwaggerResponse(200, type: typeof(StudentGroupDto))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentGroupDto>> GetStudentGroupById(long id)
        {
            var foundStudentGroup = await _studentGroupService
                    .GetStudentGroupByIdAsync(id);

            return foundStudentGroup.ToActionResult();
        }
    }
}
