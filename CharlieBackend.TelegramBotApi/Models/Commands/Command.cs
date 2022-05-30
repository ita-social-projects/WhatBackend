using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CharlieBackend.TelegramBotApi.Models.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract Task<string> Execute(Message message, TelegramBotClient client);

        public bool Contains(string command) 
        {
            return command.ToLower().Contains(this.Name);
        }
    }
}