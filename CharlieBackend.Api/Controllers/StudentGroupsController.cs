using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.Student;
using CharlieBackend.Core.Models.StudentGroup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentGroupsController : ControllerBase
    {

        private readonly IStudentGroupService _studentGroupService;

        public StudentGroupsController(IStudentGroupService studentGroupService)
        {
            _studentGroupService = studentGroupService;
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudentGroup(long id)
        {
            var x = await _studentGroupService.SearchStudentGroup(id);
            if (x == null)
                return Ok("Not Found");
            else
            {
                _studentGroupService.DeleteStudentGrop(id);
                return Ok("Done");
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutStudentGroup(StudentGroupModel studentGroup)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> PostStudentGroupController(StudentGroupModel studentGroup)
        {
            var isNameGroupTaken = await _studentGroupService.IsGroupNameTakenAsync(studentGroup.name);
            if(isNameGroupTaken) return StatusCode(409, "Student Group already exists!");

            var createdStudentGrouprModel = await _studentGroupService.CreateStudentGroupAsync(studentGroup);
            if (createdStudentGrouprModel == null) return StatusCode(422, "Invalid StudentGroup.");

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<StudentGroupModel>>> GetAllStudentGroups()
        {
            try
            {
                var studentGroupsModels = await _studentGroupService.GetAllStudentGroupsAsync();
                return Ok(studentGroupsModels);
            }
            catch { return StatusCode(500); }
        }

    }
}
