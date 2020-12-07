using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Models.StudentGroups;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class StudentGroupController : Controller
    {
        private readonly ILogger<StudentGroupController> _logger;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IStudentGroupService _studentGroupService;

        private readonly IDataProtector _protector;

        public StudentGroupController(ILogger<StudentGroupController> logger,
                                      IOptions<ApplicationSettings> config,
                                      IStudentGroupService studentGroupService,
                                      IDataProtectionProvider provider)
        {
            _logger = logger;
            _studentGroupService = studentGroupService;

            _config = config;
            _protector = provider.CreateProtector(_config.Value.Cookies.SecureKey);
        }

        [HttpGet]
        public async Task<IActionResult> AllStudentGroups()
        {
            var studentGroups = await _studentGroupService.GetAllStudentGroupsAsync(_protector.Unprotect(Request.Cookies["accessToken"]));

            return View(studentGroups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> PrepareStudentGroupForUpdate(long id)
        {
            var studentGroup = await _studentGroupService.PrepareStudentGroupUpdateAsync(id, _protector.Unprotect(Request.Cookies["accessToken"]));

            ViewBag.StudentGroup = studentGroup;

            return View("UpdateStudentGroup");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateStudentGroup(long id, StudentGroupDto data)
        {
            var updatedStudentGroup = await _studentGroupService.UpdateStudentGroupAsync(id, data, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllStudentGroups", "StudentGroup");
        }

        [HttpGet]
        public async Task<IActionResult> CreateStudentGroup()
        {
            var studentGroupData = await _studentGroupService.PrepareStudentGroupAddAsync(_protector.Unprotect(Request.Cookies["accessToken"]));

            ViewBag.StudentGroup = studentGroupData;

            return View("AddStudentGroup");
        }

        [HttpPost]
        public async Task<IActionResult> AddStudentGroup(long id, CreateStudentGroupDto data)
        {
            var updatedStudentGroup = await _studentGroupService.AddStudentGroupAsync(id, data, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllStudentGroups", "StudentGroup");
        }

        
    }
}
