using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ILogger<StudentsController> _logger;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IApiUtil _apiUtil;


        public StudentsController(ILogger<StudentsController> logger, IOptions<ApplicationSettings> config, IApiUtil apiUtil)
        {
            _logger = logger;
            _apiUtil = apiUtil;

            _config = config;
        }
     
        public async Task<IActionResult> AllStudents()
        {
            var students = await _apiUtil.GetAsync<IList<StudentDto>>($"{_config.Value.Urls.Api.Https}/api/accounts", HttpContext.Session.GetString("accessToken"));

            return View(students);
        }
    }
}
