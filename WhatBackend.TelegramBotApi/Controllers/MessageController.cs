using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WhatBackend.TelergamBot.Models;

namespace WhatBackend.TelergamBot.Controllers
{
    [ApiController]
    [Route("api/bot/message")]
    public class MessageController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Bot is up and running!";
        }

        [Route("update")]
        [HttpPost]
        public async Task<OkResult> Update([FromBody] Update update)
        {
                var commands = Bot.Commands;
                var message = update.Message ?? update.EditedMessage;
                var client = await Bot.Get();

                foreach (var command in commands)
                {
                    if (command.Contains(message.Text))
                    {
                        await command.Execute(message, client);
                        break;
                    }
                }

                return Ok();
            
        }
    }
}
