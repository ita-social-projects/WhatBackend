using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Mentor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class MentorsController : Controller
    {
        private readonly IMentorService _mentorService;

        public MentorsController(IMentorService mentorService)
        {
            _mentorService = mentorService;
        }

        public async Task<IActionResult> AllMentors()
        {
            var mentor = await _mentorService.GetAllMentorsAsync(Request.Cookies["accessToken"]);

            return View(mentor);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateMentor(long id)
        {
            var mentor = await _mentorService.GetMentorByIdAsync(id, Request.Cookies["accessToken"]);

            ViewBag.Mentor = mentor;

            return View("UpdateMentor");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateMentor(long id, UpdateMentorDto data)
        {
            var updatedStudent = await _mentorService.UpdateMentorAsync(id, data, Request.Cookies["accessToken"]);

            return RedirectToAction("AllMentors", "Mentors");
        }

        [HttpPost]
        public async Task<IActionResult> AddMentor(long id)
        {
            await _mentorService.AddMentorAsync(id, Request.Cookies["accessToken"]);

            return RedirectToAction("AllMentors", "Mentors");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DisableMentor(long id)
        {
            await _mentorService.DisableMentorAsync(id, Request.Cookies["accessToken"]);

            return RedirectToAction("AllMentors", "Mentors");
        }
    }
}
