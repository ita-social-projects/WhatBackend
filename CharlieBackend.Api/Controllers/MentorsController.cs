using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models;
using CharlieBackend.Core.Models.Mentor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [Authorize(Roles = "4")]
        [HttpPost]
        public async Task<ActionResult> PostMentor(CreateMentorModel mentorModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var isEmailTaken = await _accountService.IsEmailTakenAsync(mentorModel.Email);
            if (isEmailTaken) return StatusCode(409, "Account already exists!");

            var createdMentorModel = await _mentorService.CreateMentorAsync(mentorModel);
            if (createdMentorModel == null) return StatusCode(422, "Cannot create mentor.");

            return Ok();
        }

        [Authorize(Roles = "2, 4")]
        [HttpGet]
        public async Task<ActionResult<List<MentorModel>>> GetAllMentors()
        {
            try
            {
                var mentosModels = await _mentorService.GetAllMentorsAsync();
                return Ok(mentosModels);
            }
            catch { return StatusCode(500); }
        }

        [Authorize(Roles = "2, 4")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutMentor(long id, UpdateMentorModel mentorModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var isEmailChangableTo = await _accountService.IsEmailChangableToAsync(mentorModel.Email);
                if (!isEmailChangableTo) return StatusCode(409, "Email is already taken!");

                mentorModel.Id = id;
                var updatedCourse = await _mentorService.UpdateMentorAsync(mentorModel);
                if (updatedCourse != null) return NoContent();
                else return StatusCode(409, "Cannot update.");

            }
            catch { return StatusCode(500); }
        }

        [Authorize(Roles = "2")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DisableMentor(long id)
        {
            try
            {
                var accountId = await _mentorService.GetAccountId(id);
                if (accountId == null) return BadRequest("Unknown mentor id.");

                var isDisabled = await _accountService.DisableAccountAsync((long)accountId);
                if (isDisabled) return NoContent();
                return StatusCode(500, "Error occurred while trying to disable mentor account.");

            }
            catch { return StatusCode(401, "Bad token."); }
        }
    }
}
