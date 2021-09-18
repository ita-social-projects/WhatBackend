using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.StudentGroups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin, Secretary")]
    [Route("[controller]/[action]")]
    public class StudentGroupController : Controller
    {
        private readonly IStudentGroupService _studentGroupService;

        public StudentGroupController(IStudentGroupService studentGroupService)
        {
            _studentGroupService = studentGroupService;
        }

        [HttpGet]
        public async Task<IActionResult> AllStudentGroups()
        {
            var studentGroups = await _studentGroupService.GetAllStudentGroupsAsync();

            return View(studentGroups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> PrepareStudentGroupForUpdate(long id)
        {
            var studentGroup = await _studentGroupService.PrepareStudentGroupUpdateAsync(id);

            ViewBag.StudentGroup = studentGroup;

            return View("UpdateStudentGroup");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateStudentGroup(long id, StudentGroupDto data)
        {
            var updatedStudentGroup = await _studentGroupService.UpdateStudentGroupAsync(id, data);

            return RedirectToAction("AllStudentGroups", "StudentGroup");
        }

        [HttpGet]
        public async Task<IActionResult> CreateStudentGroup()
        {
            var studentGroupData = await _studentGroupService.PrepareStudentGroupAddAsync();

            ViewBag.StudentGroup = studentGroupData;

            return View("AddStudentGroup");
        }

        [HttpPost]
        public async Task<IActionResult> AddStudentGroup(long id, CreateStudentGroupDto data)
        {
            var updatedStudentGroup = await _studentGroupService.AddStudentGroupAsync(id, data);

            return RedirectToAction("AllStudentGroups", "StudentGroup");
        }

        
    }
}
