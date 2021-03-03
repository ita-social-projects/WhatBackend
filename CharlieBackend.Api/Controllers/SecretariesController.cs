using CharlieBackend.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Secretary;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Business.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller for operations related to secretary entity
    /// </summary>
    [Route("api/secretaries")]
    [ApiController]
    public class SecretariesController : ControllerBase
    {
        #region
        private readonly ISecretaryService _secretaryService;
        #endregion
        /// <summary>
        /// SecretariesController constructor to inject related services
        /// </summary>
        public SecretariesController(ISecretaryService secretaryService)
        {
            _secretaryService = secretaryService;
        }

        /// <summary>
        /// Gives secretary role to account and creates secretary entity
        /// </summary>
        /// <param name="accountId">Account id to approve</param>
        /// <response code="200">Successful secretary entity create</response>
        /// <response code="HTTP: 400, API: 0">Account already assigned to secretary</response>
        /// <response code="HTTP: 404, API: 3">Acccount not found</response>
        [SwaggerResponse(200, type: typeof(SecretaryDto))]
        [Authorize(Roles = "Admin")]
        [HttpPost("{accountId}")]
        public async Task<ActionResult> PostSecretary(long accountId)
        {
            var createdSecretaryDto = await _secretaryService.CreateSecretaryAsync(accountId);

            return createdSecretaryDto.ToActionResult();
        }

        /// <summary>
        /// Gets all secretaries
        /// </summary>
        /// <response code="200">Returns list of all secretaries</response>
        [Authorize(Roles = "Admin, Secretary")]
        [SwaggerResponse(200, type: typeof(IList<SecretaryDetailsDto>))]
        [HttpGet]
        public async Task<ActionResult<IList<SecretaryDetailsDto>>> GetAllSecretaries()
        {
            var secretariesDtos = await _secretaryService.GetAllSecretariesAsync();

            return Ok(secretariesDtos);
        }

        /// <summary>
        /// Gets only active secretaries
        /// </summary>
        /// <response code="200">Returns list of active secretaries</response>
        [Authorize(Roles = "Admin, Secretary")]
        [SwaggerResponse(200, type: typeof(IList<SecretaryDetailsDto>))]
        [Route("active")]
        [HttpGet]
        public async Task<ActionResult> GetActiveSecretaries()
        {
            var secretariesDtos = await _secretaryService.GetActiveSecretariesAsync();

            return secretariesDtos.ToActionResult();
        }

        /// <summary>
        /// Updates exact secretary entity
        /// </summary>
        /// <response code="200">Returns updated data of secretary</response>
        /// <response code="HTTP: 404, API: 3">Secretary not found</response>
        /// <response code="HTTP: 409, API: 5">Email already taken</response>
        [SwaggerResponse(200, type: typeof(UpdateSecretaryDto))]
        [Authorize(Roles = "Admin")]
        [HttpPut("{secretaryId}")]
        public async Task<ActionResult> PutSecretary(long secretaryId, [FromBody]UpdateSecretaryDto secretaryDto)
        {
            var updatedSecretary = await _secretaryService.UpdateSecretaryAsync(secretaryId, secretaryDto);

            return updatedSecretary.ToActionResult();
        }

        /// <summary>
        /// Disable secretary's account
        /// </summary>
        /// <response code="200">Secretary's account successfully disabled</response>
        /// <response code="HTTP: 404, API: 3">Secretary not found</response>
        /// <response code="HTTP: 409, API: 5">Secretary's account is already disabled</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{secretaryId}")]
        public async Task<ActionResult<bool>> DisableSecretary(long secretaryId)
        {
            var isDisabled = await _secretaryService.DisableSecretaryAsync(secretaryId);

            return isDisabled.ToActionResult();
        }

        /// <summary>
        /// Enable secretary's account
        /// </summary>
        /// <response code="200">Secretary's account successfully enabled</response>
        /// <response code="HTTP: 404, API: 3">Secretary not found</response>
        /// <response code="HTTP: 409, API: 5">Secretary's account is already enabled</response>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{secretaryId}")]
        public async Task<ActionResult> EnableSecretary(long secretaryId)
        {
            var isDisabled = await _secretaryService.EnableSecretaryAsync(secretaryId);

            return isDisabled.ToActionResult();
        }
    }
}
