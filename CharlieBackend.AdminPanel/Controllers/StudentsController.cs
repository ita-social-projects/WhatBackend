using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Models.Students;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Student;
using Microsoft.AspNetCore.Authorization;
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


        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;

        }

        public async Task<IActionResult> AllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync(Request.Cookies["accessToken"]);

            return View(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateStudent(long id)
        {
            var student = await _studentService.GetStudentByIdAsync(id, Request.Cookies["accessToken"]);

            ViewBag.Student = student;

            return View("UpdateStudent");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateStudent(long id, UpdateStudentDto data)
        {
            var updatedStudent = await _studentService.UpdateStudentAsync(id, data, Request.Cookies["accessToken"]);

            return RedirectToAction("AllStudents", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(long id)
        {
            var addedStudent = await _studentService.AddStudentAsync(id, Request.Cookies["accessToken"]);

            return RedirectToAction("AllStudents", "Students");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DisableStudent(long id)
        {
            var disabledStudent = await _studentService.DisableStudentAsync(id, Request.Cookies["accessToken"]);

            return RedirectToAction("AllStudents", "Students");
        }

    }
}
