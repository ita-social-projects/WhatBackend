using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manipulate telegram functions
    /// </summary>
    [Route("api/v{version:apiVersion}/accounts")]
    [ApiVersion("2.0")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        #region private readonly variables
        private readonly ITelegramService _telegramService;
        #endregion

        /// <summary>
        /// Telegram controller constructor
        /// </summary>
        public TelegramController(IAccountService accountService, ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        /// <summary>
        /// Get link for Telegram account synchronization
        /// </summary>
        /// <returns>URL</returns>
        //[SwaggerResponse(200, type: typeof(SignInResponse))]
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [Route("telegram")]
        [HttpGet]
        public async Task<ActionResult<string>> GetTelegramBotLink()
        {
            var link = await _telegramService.GetTelegramBotLink();
            return link.ToActionResult();
        }

        /// <summary>
        /// Syncronized telegram account with user account from database
        /// </summary>
        /// <response code="200">Successful return an updated account entity</response>
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [Route("telegram/sync")]
        [HttpPost]
        public async Task<ActionResult<Account>> AccountSync(string telegramToken, string telegramId)
        {
            var syncronizedAccount = await _telegramService.SynchronizeTelegramAccount(telegramToken, telegramId);
            return syncronizedAccount.ToActionResult();
        }

        //todo: patch
        /// <summary>
        /// Deletes expired Telegram tokens from database
        /// </summary>
        /// <returns>true</returns>
        [Authorize(Roles = "Admin")]
        [Route("telegram/clear")]
        [HttpPatch]
        public async Task<bool> ClearOldTelegramTokens()
        {
            return await _telegramService.ClearOldTelegramTokens();
        }
    }
}
