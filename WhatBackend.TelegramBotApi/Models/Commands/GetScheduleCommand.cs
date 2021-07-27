using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WhatBackend.TelergamBot.Models.Commands
{
    public class GetScheduleCommand : Command
    {
        public override string Name => "Schedule";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            //TODO: Inject schedule service and get data
        }
    }
}