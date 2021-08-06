using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Business.Services.Interfaces;


namespace WhatBackend.TelergamBot.Models.Commands
{
    public class GetMarkCommand : Command
    {
        private readonly IDashboardService _dashboardService;
        public override string Name => "Mark";
        public GetMarkCommand(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public override async Task Execute(Message message, TelegramBotClient client)
        { 
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            var userId = 1;
            var request = new DashboardAnalyticsRequestDto<StudentResultType> {
                StartDate = default(DateTime),
                FinishDate = default(DateTime),
                IncludeAnalytics = new StudentResultType[0]
            };

            var results = await _dashboardService.GetStudentResultAsync(userId, request);

            var recentMarks = results.Data.AverageStudentsMarks;

            await client.SendTextMessageAsync(chatId, recentMarks.ToString(), replyToMessageId: messageId);
        }
    }
}