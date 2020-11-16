using CharlieBackend.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Secretary;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Core.Models.ResultModel;
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
        [HttpPost("{accountId}")]
        public async Task<ActionResult> PostSecretary(long accountId)
        {
            var createdSecretaryDto = await _secretaryService.CreateSecretaryAsync(accountId);

            return createdSecretaryDto.ToActionResult();
        }

        [Authorize(Roles = "Admin, Secretary")]
        [HttpGet]
        public async Task<ActionResult> GetAllSecretaries()
        {
            var secretariesDtos = await _secretaryService.GetAllSecretariesAsync();

            return secretariesDtos.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{secretaryId}")]
        public async Task<ActionResult> PutSecretary(long secretaryId, UpdateSecretaryDto secretaryDto)
        {
            var updatedSecretary = await _secretaryService.UpdateSecretaryAsync(secretaryId, secretaryDto);

            return updatedSecretary.ToActionResult();
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("{secretaryId}")]
        public async Task<ActionResult> DisableSecretary(long secretaryId)
        {

            var accountId = await _secretaryService.GetAccountId(secretaryId);

            if (accountId == null)
            {
                return BadRequest("Unknown secretary id.");
            }

            var isDisabled = await _accountService.DisableAccountAsync((long)accountId);

            if (isDisabled)
            {
                return Ok(await _secretaryService.GetSecretaryByIdAsync(secretaryId));
            }

            return StatusCode(500, "Error occurred while trying to disable secretary account.");
        }

    }
}
