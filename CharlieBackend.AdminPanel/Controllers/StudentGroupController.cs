using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin/student_groups")]
    public class StudentGroupController : Controller
    {
        private readonly ILogger<StudentGroupController> _logger;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IStudentGroupService _studentGroupService;


        public StudentGroupController(ILogger<StudentGroupController> logger, IOptions<ApplicationSettings> config, IStudentGroupService studentGroupService)
        {
            _logger = logger;
            _studentGroupService = studentGroupService;

            _config = config;
        }

        public async Task<IActionResult> AllStudentGroups()
        {
            var studentGroups = await _studentGroupService.GetAllStudentGroups(Request.Cookies["accessToken"]);

            return View(studentGroups);
        }
    }
}
