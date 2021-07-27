using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using WhatBackend.TelergamBot.Models.Commands;

namespace WhatBackend.TelergamBot.Models
{
    public class Bot
    {
        private static TelegramBotClient _client;
        private static List<Command> _commandsList;

        public static IReadOnlyCollection<Command> Commands { get => _commandsList.AsReadOnly(); }

        public static async Task<TelegramBotClient> Get()
        {
            if (_client!=null)
            {
                return _client;
            }

            _commandsList = new List<Command>();
            _commandsList.Add(new HelloCommand());

            _client = new TelegramBotClient(AppSettings.Key);
            var hook = AppSettings.Url + "/api/bot/message/update";
            await _client.SetWebhookAsync(hook);

            return _client;
        }
    }
}