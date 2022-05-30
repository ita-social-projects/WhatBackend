using CharlieBackend.Business.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CharlieBackend.TelegramBotApi.Models.Commands
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
                "Press the button 'MENU' below the input field to open menu and begin dealing with me:\n";

            return (await client.SendTextMessageAsync(chatId, 
                response, replyToMessageId: messageId, replyMarkup: GetMainMenu())).Text;
        }

        private IReplyMarkup GetMainMenu()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{new KeyboardButton { Text = "MENU" } }
                },
                ResizeKeyboard = true
            };
        }
    }
}
