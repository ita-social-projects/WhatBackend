using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBot
{
    [ApiController]
    [Route("bot/message")]
    public class MessageController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            Response.Cookies.Append("test", "testCookie");
            return "Bot is up and running";
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromServices] HandleUpdateService handleUpdateService,
                                              [FromBody] Update update)
        {
            await handleUpdateService.EchoAsync(update);    
            return Ok();
        }
    }
}
