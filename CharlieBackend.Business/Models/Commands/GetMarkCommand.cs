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
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;
        private readonly IDashboardService _dashboardService;
        public override string Name => "mark";
        public GetMarkCommand(IAccountService accountService,
            IStudentService studentService,
            IDashboardService dashboardService)
        {
            _accountService = accountService;
            _studentService = studentService;
            _dashboardService = dashboardService;
        }
        public override async Task<string> Execute(Message message, TelegramBotClient client)
        { 
            var chatId = message.Chat.Id;
            string response = string.Empty;

            var account = await _accountService
                .GetAccountByTelegramId(chatId);
            var student = await _studentService
                .GetStudentByAccountIdAsync(account.Id);
            var results = await _dashboardService
                .GetStudentClassbookAsync(student.Data.Id,
                new DashboardAnalyticsRequestDto<ClassbookResultType>()
                {
                    FinishDate = DateTime.UtcNow,
                    StartDate = DateTime.UtcNow - new TimeSpan(7, 0, 0, 0),
                    IncludeAnalytics = new ClassbookResultType[] 
                    { 
                        ClassbookResultType.StudentMarks, 
                        ClassbookResultType.StudentPresence 
                    }
                });

            //implement new logic here (after rework of DashboardService)

            return (await client.SendTextMessageAsync(chatId,
                response)).Text;
        }
    }
}