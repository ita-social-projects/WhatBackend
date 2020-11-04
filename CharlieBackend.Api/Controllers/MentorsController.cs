using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.DTO;
using CharlieBackend.Core.DTO.Mentor;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Core;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/mentors")]
    [ApiController]
    public class MentorsController : ControllerBase
    {
        #region
        private readonly IMentorService _mentorService;
        private readonly IAccountService _accountService;
        #endregion

        public MentorsController(IMentorService mentorService, IAccountService accountService)
        {
            _mentorService = mentorService;
            _accountService = accountService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}")]
        public async Task<ActionResult> PostMentor(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var createdMentorModel = await _mentorService.CreateMentorAsync(id);

            if (createdMentorModel == null)
            {
                return Result<MentorDto>.Error(ErrorCode.UnprocessableEntity,
                    "Cannot create mentor.").ToActionResult();
            }

            return createdMentorModel.ToActionResult(); ;
        }

        [Authorize(Roles = "Mentor, Admin")]
        [HttpGet]
        public async Task<ActionResult<List<MentorDto>>> GetAllMentors()
        {

            var mentorsModels = await _mentorService.GetAllMentorsAsync();

            return Ok(mentorsModels);
        }
        
        [Authorize(Roles = "Mentor, Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutMentor(long id, UpdateMentorDto mentorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var isEmailChangableTo = await _accountService.IsEmailChangableToAsync(mentorModel.Email);

            if (!isEmailChangableTo)
            {
                return StatusCode(409, "Email is already taken!");
            }

            var updatedMentor = await _mentorService.UpdateMentorAsync(id, mentorModel);

            if (updatedMentor != null)
            {
                return Ok(updatedMentor);
            }

            return StatusCode(409, "Cannot update.");
        } 

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DisableMentor(long id)
        {

            var accountId = await _mentorService.GetAccountId(id);

            if (accountId == null)
            {
                return BadRequest("Unknown mentor id.");
            }

            var isDisabled = await _accountService.DisableAccountAsync((long)accountId);

            if (isDisabled)
            {
                return NoContent();
            }

            return StatusCode(500, "Error occurred while trying to disable mentor account.");
        }
    }
}
