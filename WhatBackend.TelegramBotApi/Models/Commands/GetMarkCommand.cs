using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WhatBackend.TelergamBot.Models.Commands
{
    public class GetMarkCommand : Command
    {
        public override string Name => "Mark";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            //TODO: Inject service and get data
        }
    }
}