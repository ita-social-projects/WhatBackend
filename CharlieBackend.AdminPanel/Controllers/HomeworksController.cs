using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Homework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeworksController : Controller
    {
        private readonly IHomeworkService _homeworkService;

        public HomeworksController(IHomeworkService homeworkService)
        {
            _homeworkService = homeworkService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetHomework(long id)
        {
            var homework = await _homeworkService.GetHomeworkById(id);

            return View(homework);
        }


        [HttpGet]
        public async Task<IActionResult> CreateHomework()
        {
            return View("AddNewHomework");
        }

        [HttpPost]
        public async Task<IActionResult> PostHomework(HomeworkDto homework)
        {
            await _homeworkService.AddHomeworkEndpoint(homework);

            return RedirectToAction("Index", nameof(HomeworksController));
        }

        [HttpPut]
        public async Task<IActionResult> PutHomework(long id, HomeworkDto homework)
        {
            await _homeworkService.UpdateHomeworkEndpoint(id, homework);

            return RedirectToAction("Index", nameof(HomeworksController));
        }
    }
}
