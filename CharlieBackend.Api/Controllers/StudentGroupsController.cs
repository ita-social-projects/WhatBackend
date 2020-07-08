using System.Collections.Generic;
using System.Linq;
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
