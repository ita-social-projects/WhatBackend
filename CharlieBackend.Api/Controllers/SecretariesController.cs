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
        [HttpPost]
        public async Task<ActionResult> CreateSecretary(CreateSecretaryDto secretaryDto)
        {
            var createdSecretaryDto = await _secretaryService.CreateSecretaryAsync(secretaryDto);

            return createdSecretaryDto.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllSecretaries()
        {
            var secretariesDtos = await _secretaryService.GetAllSecretariesAsync();

            return secretariesDtos.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{secretaryId}")]
        public async Task<ActionResult> UpdateSecretary(long secretaryId, UpdateSecretaryDto secretaryDto)
        {
            secretaryDto.Id = secretaryId;
            var updatedSecretary = await _secretaryService.UpdateSecretaryAsync(secretaryDto);

            return updatedSecretary.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{secretaryId}")]
        public async Task<ActionResult> DisableSecretary(long secretaryId)
        {
            var isDisabled = await _secretaryService.DisableSecretaryAsync(secretaryId);

            return isDisabled.ToActionResult();
        }

    }
}
