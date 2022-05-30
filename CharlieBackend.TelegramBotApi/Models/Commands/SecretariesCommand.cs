using CharlieBackend.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CharlieBackend.TelegramBotApi.Models.Commands
{
    public class SecretariesCommand : Command
    {
        private readonly ISecretaryService _secretaryService;

        public override string Name => "secretaries";

        public SecretariesCommand(ISecretaryService secretaryService)
        {
            _secretaryService = secretaryService;
        }

        public override async Task<string> Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            string response = string.Empty;

            var secretaries = await _secretaryService
                .GetAllSecretariesAsync();

            foreach (var secretary in secretaries)
            {
                response += secretary.FirstName;
                response += " ";
                response += secretary.LastName;
                response += " ";
                response += secretary.Email;
                response += "\n";
            }

            return (await client.SendTextMessageAsync(chatId,
                response)).Text;
        }
    }
}
