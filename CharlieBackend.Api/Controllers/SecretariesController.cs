using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Secretary;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/secretaries")]
    [ApiController]
    public class SecretariesController : ControllerBase
    {
        #region
        private readonly ISecretaryService _secretaryService;
        private readonly IAccountService _accountService;
        #endregion

        public SecretariesController(ISecretaryService secretaryService, IAccountService accountService)
        {
            _secretaryService = secretaryService;
            _accountService = accountService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateSecretary(CreateSecretaryDto secretaryDto)
        {

            var isEmailTaken = await _accountService.IsEmailTakenAsync(secretaryDto.Email);

            if (isEmailTaken)
            {
                return StatusCode(409, "Account already exists!");
            }

            var createdSecretaryDto = await _secretaryService.CreateSecretaryAsync(secretaryDto);

            if (createdSecretaryDto == null)
            {
                return StatusCode(422, "Cannot create secretary.");
            }

            return Ok(createdSecretaryDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<SecretaryDto>>> GetAllSecretaries()
        {

            var secretariesDtos = await _secretaryService.GetAllSecretariesAsync();

            return Ok(secretariesDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSecretary(long id, UpdateSecretaryDto secretaryDto)
        {

            var isEmailChangableTo = await _accountService
                    .IsEmailChangableToAsync(secretaryDto.Email);

            if (!isEmailChangableTo)
            {
                return StatusCode(409, "Email is already taken!");
            }

            secretaryDto.Id = id;
            var updatedSecretary = await _secretaryService.UpdateSecretaryAsync(secretaryDto);

            if (updatedSecretary != null)
            {
                return Ok(updatedSecretary);
            }

            return StatusCode(409, "Cannot update.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DisableSecretary(long id)
        {

            var accountId = await _secretaryService.GetAccountId(id);

            if (accountId == null)
            {
                return BadRequest("Unknown secretary id.");
            }

            var isDisabled = await _accountService.DisableAccountAsync((long)accountId);

            if (isDisabled)
            {
                return NoContent();
            }

            return StatusCode(500, "Error occurred while trying to disable secretary account.");
        }

    }
}
