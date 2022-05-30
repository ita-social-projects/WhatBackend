using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CharlieBackend.TelegramBotApi.Models.Commands
{
    public class GetScheduleCommand : Command
    {
        public override string Name => "Schedule";

        public override async Task<string> Execute(Message message, TelegramBotClient client)
        {
            return string.Empty;
            //TODO: Inject schedule service and get data
        }
    }
}