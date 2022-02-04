using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.Entities;
using CharlieBackend.Panel.Models.Mentor;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary, Mentor")]
    [Route("[controller]/[action]")]
    public class MentorsController : Controller
    {
        private readonly IMentorService _mentorService;
        private readonly ICurrentUserService _currentUserService;

        public MentorsController(IMentorService mentorService, ICurrentUserService currentUserService)
        {
            _mentorService = mentorService;
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> AllMentors()
        {
            IEnumerable<MentorViewModel> mentor = null;

            if (_currentUserService.Role == UserRole.Admin || _currentUserService.Role == UserRole.Secretary)
            {
                mentor = await _mentorService.GetAllMentorsAsync();
            }
            else if(_currentUserService.Role == UserRole.Mentor)
            {
                mentor = await _mentorService.GetAllActiveMentorsAsync();
            }

            return View(mentor);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateMentor(long id)
        {
            var mentor = await _mentorService.GetMentorByIdAsync(id);

            ViewBag.Mentor = mentor;

            return View("UpdateMentor");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DisableMentor(long id)
        {
            await _mentorService.DisableMentorAsync(id);

            return RedirectToAction("AllMentors", "Mentors");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EnableMentor(long id)
        {
            await _mentorService.EnableMentorAsync(id);

            return RedirectToAction("AllMentors", "Mentors");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateMentor(long id, UpdateMentorDto data)
        {
            var updatedStudent = await _mentorService.UpdateMentorAsync(id, data);

            return RedirectToAction("AllMentors", "Mentors");
        }

        [HttpPost]
        public async Task<IActionResult> AddMentor(long id)
        {
            await _mentorService.AddMentorAsync(id);

            return RedirectToAction("AllMentors", "Mentors");
        }
    }
}
