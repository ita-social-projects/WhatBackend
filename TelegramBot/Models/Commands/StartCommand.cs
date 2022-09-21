using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Models.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "start";

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
                //ToDo: implement api call for sync with acc
                //var result = 
            }

            response += "Hello! I'm a Telegram bot of WHAT. " +
              "Press the button 'MENU' below the input field to open menu and begin dealing with me:\n";

            return (await client.SendTextMessageAsync(chatId,
                response, replyToMessageId: messageId, replyMarkup: GetMainMenu())).Text;
        }

        private IReplyMarkup GetMainMenu()
        {
            return new ReplyKeyboardMarkup(new KeyboardButton("MENU"))
            {
                ResizeKeyboard = true
            };
        }
    }
}
