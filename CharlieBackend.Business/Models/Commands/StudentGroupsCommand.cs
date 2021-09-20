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
    public class StudentGroupsCommand : Command
    {
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;
        private readonly IMentorService _mentorService;
        public override string Name => "studentgroups";
        public StudentGroupsCommand(IAccountService accountService,
            IStudentService studentService,
            IMentorService mentorService)
        {
            _accountService = accountService;
            _studentService = studentService;
            _mentorService = mentorService;
        }

        public override async Task<string> Execute(Message message, 
            TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            string response = string.Empty;

            var account = await _accountService
                .GetAccountByTelegramId(chatId);

            if(account.Role.Is(Core.Entities.UserRole.Student))
            {
                response += "Student groups, which list you as a student:\n";
                var student = await _studentService
                    .GetStudentByAccountIdAsync(account.Id);
                var groups = await _studentService
                    .GetStudentStudyGroupsByStudentIdAsync(student.Data.Id);
                foreach (var group in groups.Data)
                {
                    response += "\n";
                    response += group.Name; 
                }
            }

            if (account.Role.Is(Core.Entities.UserRole.Mentor))
            {
                if(response != string.Empty)
                {
                    response += "\n\n";
                }
                response += "Student groups, which list you as a mentor:\n";
                var mentor = await _mentorService
                    .GetMentorByAccountIdAsync(account.Id);
                var groups = await _mentorService
                    .GetMentorStudyGroupsByMentorIdAsync(mentor.Data.Id);
                foreach (var group in groups)
                {
                    response += "\n";
                    response += group.Name;
                }
            }

            if(response == string.Empty)
            {
                response += "You aren't listed in any student group.\n";
            }

            return (await client.SendTextMessageAsync(chatId,
                response, replyToMessageId: messageId)).Text;
        }
    }
}
