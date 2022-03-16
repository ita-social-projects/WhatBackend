using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{
    [Route("[controller]/[action]")]
    public class StudentGroupController : Controller
    {
        private readonly IStudentGroupService _studentGroupService;

        public StudentGroupController(IStudentGroupService studentGroupService)
        {
            _studentGroupService = studentGroupService;
        }

        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpGet]
        public async Task<IActionResult> AllStudentGroups(bool isAllGroups = true)
        {
            ViewBag.IsAllGroups = isAllGroups;

            var studentGroups = await _studentGroupService.GetAllStudentGroupsAsync(isAllGroups);

            return View(studentGroups);
        }

        [Authorize(Roles = "Secretary, Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> PrepareStudentGroupForUpdate(long id)
        {
            var studentGroup = await _studentGroupService.PrepareStudentGroupUpdateAsync(id);

            ViewBag.StudentGroup = studentGroup;

            return View("UpdateStudentGroup");
        }

        [Authorize(Roles = "Secretary, Admin")]
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateStudentGroup(long id, StudentGroupDto data)
        {
            var updatedStudentGroup = await _studentGroupService.UpdateStudentGroupAsync(id, data);

            return RedirectToAction("AllStudentGroups", "StudentGroup");
        }

        [Authorize(Roles = "Secretary, Admin")]
        [HttpGet]
        public async Task<IActionResult> CreateStudentGroup()
        {
            var studentGroupData = await _studentGroupService.PrepareStudentGroupAddAsync();

            ViewBag.StudentGroup = studentGroupData;

            return View("AddStudentGroup");
        }

        [Authorize(Roles = "Secretary, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddStudentGroup(long id, CreateStudentGroupDto data)
        {
            var updatedStudentGroup = await _studentGroupService.AddStudentGroupAsync(id, data);

            return RedirectToAction("AllStudentGroups", "StudentGroup");
        }

    }
}
