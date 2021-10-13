using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CharlieBackend.Business.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CharlieBackend.Business.Models.Commands
{
    public class MentorsCommand : Command
    {
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IMentorService _mentorService;

        public override string Name => "mentors";

        public MentorsCommand(IAccountService accountService,
            IStudentService studentService,
            IStudentGroupService studentGroupService,
            IMentorService mentorService)
        {
            _accountService = accountService;
            _studentService = studentService;
            _studentGroupService = studentGroupService;
            _mentorService = mentorService;
        }

        public override async Task<string> Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            string response = string.Empty;

            var account = await _accountService
                .GetAccountByTelegramId(chatId);
            var student = await _studentService
                .GetStudentByAccountIdAsync(account.Id);
            var groups = (await _studentService
                .GetStudentStudyGroupsByStudentIdAsync(student.Data.Id)).Data;

            foreach (var group in groups)
            {
                var groupInfo = await _studentGroupService
                    .GetStudentGroupByIdAsync(group.Id);

                response += groupInfo.Data.Name + "\n\n";

                foreach (var mentorId in groupInfo.Data.MentorIds)
                {
                    var mentor = await _mentorService
                        .GetMentorByIdAsync(mentorId);

                    response += mentor.Data.FirstName;
                    response += " ";
                    response += mentor.Data.LastName;
                    response += " ";
                    response += mentor.Data.Email;
                    response += "\n";
                }

                response += "\n";
            }

            return (await client.SendTextMessageAsync(chatId,
                response)).Text;
        }
    }
}
