using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CharlieBackend.TelegramBotApi.Models.Commands
{
    public class HelloCommand : Command
    {
        public override string Name => "Hello";

        public override async Task<string> Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            return (await client.SendTextMessageAsync(chatId, "Hello!", replyToMessageId: messageId)).Text;
        }
    }
}