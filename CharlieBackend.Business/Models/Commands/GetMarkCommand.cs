using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Models.Commands
{

    /// <summary>
    /// This command uses UserID to Send him message about his recent marks
    /// </summary>
    public class GetMarkCommand : Command
    {
        private readonly IDashboardService _dashboardService;
        public override string Name => "Mark";
        public GetMarkCommand(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public override async Task<string> Execute(Message message, TelegramBotClient client)
        { 
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            //var userId = 1;
            //var request = new DashboardAnalyticsRequestDto<StudentResultType>
            //{
            //    StartDate = default(DateTime),
            //    FinishDate = default(DateTime),
            //    IncludeAnalytics = new StudentResultType[0]
            //};
            //var results = await _dashboardService.GetStudentResultAsync(userId, request);

            var results = new Result<StudentMarkDto>  //temporary stub to make this work
            {
                Data = new StudentMarkDto
                {
                    //StudentGroupId = 1,
                    StudentId = 1,
                    LessonId = 1,
                    //CourseId = 1,
                    StudentMark = 10
                },
                Error = null
            };

            var recentMark = results.Data.StudentMark;
            var replyMessage = string.Format("Your recent mark is {0}", recentMark.ToString());
            return (await client.SendTextMessageAsync(chatId, replyMessage, replyToMessageId: messageId)).Text;

        }
    }
}