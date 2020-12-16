﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Mentor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class MentorsController : Controller
    {
        private readonly IMentorService _mentorService;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IDataProtector _protector;

        public MentorsController(IMentorService mentorService,
                                  IOptions<ApplicationSettings> config,
                                  IDataProtectionProvider provider)
        {
            _mentorService = mentorService;
            _config = config;
            _protector = provider.CreateProtector(_config.Value.Cookies.SecureKey);

        }

        public async Task<IActionResult> AllMentors()
        {
            var mentor = await _mentorService.GetAllMentorsAsync(_protector.Unprotect(Request.Cookies["accessToken"]));

            return View(mentor);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateMentor(long id)
        {
            var mentor = await _mentorService.GetMentorByIdAsync(id, _protector.Unprotect(Request.Cookies["accessToken"]));

            ViewBag.Mentor = mentor;

            return View("UpdateMentor");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateMentor(long id, UpdateMentorDto data)
        {
            var updatedStudent = await _mentorService.UpdateMentorAsync(id, data, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllMentors", "Mentors");
        }

        [HttpPost]
        public async Task<IActionResult> AddMentor(long id)
        {
            await _mentorService.AddMentorAsync(id, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllMentors", "Mentors");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DisableMentor(long id)
        {
            await _mentorService.DisableMentorAsync(id, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllMentors", "Mentors");
        }
    }
}
