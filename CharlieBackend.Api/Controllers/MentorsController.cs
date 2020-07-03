using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/mentor")]
    [ApiController]
    public class MentorsController : ControllerBase
    {
        private readonly IMentorService _mentorService;
        private readonly IAccountService _accountService;

        public MentorsController(IMentorService mentorService, IAccountService accountService)
        {
            _mentorService = mentorService;
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<ActionResult> PostMentor(MentorModel mentorModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var isEmailTaken = await _accountService.IsEmailTakenAsync(mentorModel.login);
            if (isEmailTaken) return StatusCode(409, "Account already exists!");

            var createdMentorModel = await _mentorService.CreateMentorAsync(mentorModel);
            if (createdMentorModel == null) return StatusCode(422, "Invalid courses.");

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<MentorModel>>> GetAllMentors()
        {
            try
            {
                var mentosModels = await _mentorService.GetAllMentorsAsync();
                return Ok(mentosModels);
            } catch { return StatusCode(500); }
        }
    }
}
