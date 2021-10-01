using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CharlieBackend.Business.Models.Commands
{
    public class CoursesCommand : Command
    {
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;
        private readonly IMentorService _mentorService;
        public override string Name => "courses";

        public CoursesCommand(IAccountService accountService,
            IStudentService studentService,
            IMentorService mentorService)
        {
            _accountService = accountService;
            _studentService = studentService;
            _mentorService = mentorService;
        }

        public override async Task<string> Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            string response = string.Empty;

            var account = await _accountService
                .GetAccountByTelegramId(chatId);

            if (account.Role.Is(Core.Entities.UserRole.Mentor))
            {
                if (response != string.Empty)
                {
                    response += "\n\n";
                }
                response += "Courses, which list you as a mentor:\n";
                var mentor = await _mentorService
                    .GetMentorByAccountIdAsync(account.Id);
                var courses = await _mentorService
                    .GetMentorCoursesByMentorIdAsync(mentor.Data.Id);
                foreach (var course in courses)
                {
                    response += "\n";
                    response += course.Name;
                }
            }

            if (response == string.Empty)
            {
                response += "You aren't a mentor of any course.\n";
            }

            return (await client.SendTextMessageAsync(chatId,
                response)).Text;
        }
    }
}
