using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.StudentGroups;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/student_groups")]
    [ApiController]
    public class StudentGroupsController : ControllerBase
    {

        private readonly IStudentGroupService _studentGroupService;

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

        [Authorize(Roles = "2, 4")]
        [HttpPost]
        public async Task<ActionResult<StudentGroupDto>> PostStudentGroup(CreateStudentGroupDto studentGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var isStudentGroupNameChangable = await _studentGroupService
                    .IsGroupNameTakenAsync(studentGroup.Name);

            if (!isStudentGroupNameChangable)
            {
                return StatusCode(409, "Student Group already exists!");
            }

            var resStudentGroup = await _studentGroupService.CreateStudentGroupAsync(studentGroup);

            if (resStudentGroup == null)
            {
                return StatusCode(422, "Cannot create student group.");
            }

            return Ok(resStudentGroup);
        }

        [Authorize(Roles = "2, 4")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateStudentGroupDto>> PutStudentGroup(long id, UpdateStudentGroupDto studentGroupDto)
        {

            var isStudentGroupNameChangable = await _studentGroupService
                       .IsGroupNameTakenAsync(studentGroupDto.Name);

            if (!isStudentGroupNameChangable)
            {
                return StatusCode(409, "Student group name is already taken!");
            }

            var updatedStudentGroup = await _studentGroupService
                    .UpdateStudentGroupAsync(id, studentGroupDto);

            if (updatedStudentGroup != null)
            {
                return Ok(updatedStudentGroup);
            }

            return StatusCode(409, "Cannot update.");
        }

        [Authorize(Roles = "2, 4")]
        [HttpPut("{id}/students")]
        public async Task<ActionResult<UpdateStudentsForStudentGroup>> PutStudentsOfStudentGroup(long id, UpdateStudentsForStudentGroup studentGroupDto) 
        {

            var updatedStudentGroup = await _studentGroupService
                    .UpdateStudentsForStudentGroupAsync(id, studentGroupDto);

            if (updatedStudentGroup != null)
            {
                return Ok(updatedStudentGroup);
            }

            return StatusCode(409, "Cannot update.");
        }

        [Authorize(Roles = "2, 4")]
        [HttpGet]
        public async Task<ActionResult<List<StudentGroupDto>>> GetAllStudentGroups()
        {

            var studentGroupsres = await _studentGroupService.GetAllStudentGroupsAsync();

            return Ok(studentGroupsres);
        }

        [Authorize(Roles = "2, 4")]
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentGroupDto>> GetStudentGroupById(long id)
        {
            var foundStudentGroup = await _studentGroupService
                    .GetStudentGroupByIdAsync(id);

            if (foundStudentGroup == null)
            { 
                return BadRequest("No such student group.");
            }

            return foundStudentGroup;
        }
    }
}
