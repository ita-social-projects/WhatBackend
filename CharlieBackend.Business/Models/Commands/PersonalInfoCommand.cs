using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CharlieBackend.Business.Models.Commands
{
    public class PersonalInfoCommand : Command
    {
        private readonly IAccountService _accountService;
        public override string Name => "personalinfo";

        public PersonalInfoCommand(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public override async Task<string> Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            string response = string.Empty;

            var account = await _accountService
                .GetAccountByTelegramId(chatId);

            response += account.FirstName + " " + account.LastName + "\n";
            response += account.Email + "\n";

            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                //TODO: check if second part of conditional expression
                //is really needed
                if(account.Role.Is((UserRole)role)
                    && (UserRole)role != UserRole.NotAssigned)
                {
                    response += role + " ";
                }
            }

            return (await client.SendTextMessageAsync(chatId,
                response, replyToMessageId: messageId)).Text;
        }
    }
}
