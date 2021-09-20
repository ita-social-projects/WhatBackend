using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CharlieBackend.Business.Models.Commands
{
    public class StartCommand : Command
    {
        private readonly IAccountService _accountService;
        public override string Name => "start";
        public StartCommand(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public override async Task<string> Execute(Message message, TelegramBotClient client)
        {
            var parameters = message.Text.Split(' ');
            string token;
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            string response = string.Empty;

            if (parameters.Length > 1)
            {
                token = parameters[1];
                var result = await _accountService
                    .SynchronizeTelegramAccount(token, chatId.ToString());

                if (result.Error != null)
                {
                    response += result.Error.Message;
                    response += "\n";
                }

            }

            response += "Hello! I'm a Telegram bot of WHAT. " +
                "Here's a list of my commands:\n" +
                "/start - get this message again\n" +
                "/studentgroups - get list of your student groups\n" +
                "/courses - get list of courses, which list you as a mentor\n" +
                "/personalinfo - get personal info\n" +
                "/classmates - get list of my classmates\n";

            return (await client.SendTextMessageAsync(chatId, 
                response, replyToMessageId: messageId)).Text;
        }
    }
}
