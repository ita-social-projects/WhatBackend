using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin/students")]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;


        public StudentsController( IStudentService studentService)
        {
            _studentService = studentService;

        }
     
        public async Task<IActionResult> AllStudents()
        {
            var students = await _studentService.GetAllStudents(Request.Cookies["accessToken"]);

            return View(students);
        }
    }
}
