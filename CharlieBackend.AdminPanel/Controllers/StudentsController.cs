using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Models.Students;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Student;
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
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IDataProtector _protector;

        public StudentsController(IStudentService studentService,
                                  IOptions<ApplicationSettings> config,
                                  IDataProtectionProvider provider)
        {
            _studentService = studentService;
            _config = config;
            _protector = provider.CreateProtector(_config.Value.Cookies.SecureKey);

        }

        public async Task<IActionResult> AllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync(_protector.Unprotect(Request.Cookies["accessToken"]));

            return View(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateStudent(long id)
        {
            var student = await _studentService.GetStudentByIdAsync(id, _protector.Unprotect(Request.Cookies["accessToken"]));

            ViewBag.Student = student;

            return View("UpdateStudent");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateStudent(long id, UpdateStudentDto data)
        {
            var updatedStudent = await _studentService.UpdateStudentAsync(id, data, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllStudents", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(long id)
        {
            var addedStudent = await _studentService.AddStudentAsync(id, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllStudents", "Students");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DisableStudent(long id)
        {
            var disabledStudent = await _studentService.DisableStudentAsync(id, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllStudents", "Students");
        }

    }
}
