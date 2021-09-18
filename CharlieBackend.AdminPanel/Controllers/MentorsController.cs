using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Mentor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin, Secretary")]
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
            var mentor = await _mentorService.GetAllMentorsAsync();

            return View(mentor);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateMentor(long id)
        {
            var mentor = await _mentorService.GetMentorByIdAsync(id);

            ViewBag.Mentor = mentor;

            return View("UpdateMentor");
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
    }
}
