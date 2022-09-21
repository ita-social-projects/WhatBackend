using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot.Models.Commands;

namespace TelegramBot.Models
{
    public class Bot
    {
        #region private variables
        private static TelegramBotClient _client;
        private static List<Command> _commands;
        #endregion

        #region property
        public string Name { get; }
        //Todo: check if property only for read (maybe its working without IReadOnlyCollection)
        public static IReadOnlyCollection<Command> Commands 
        { 
            get => _commands.AsReadOnly(); 
        }
        #endregion

        public static async Task<TelegramBotClient> Get(IServiceProvider services)
        {
            var startCommand = services.GetRequiredService<StartCommand>();

            _commands = new List<Command>();
            _commands.Add(startCommand);

            _client = new TelegramBotClient(BotSettings.Key);
            var hook = BotSettings.Url + "/bot/message/update";
            await _client.SetWebhookAsync(hook);

            return _client;
        }
    }
}
