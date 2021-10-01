using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CharlieBackend.Business.Models.Commands
{
    public class ClassmatesCommand : Command
    {
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;
        private readonly IStudentGroupService _studentGroupService;
        public override string Name => "classmates";

        public ClassmatesCommand(IAccountService accountService,
            IStudentService studentService,
            IStudentGroupService studentGroupService)
        {
            _accountService = accountService;
            _studentService = studentService;
            _studentGroupService = studentGroupService;
        }

        //TODO: fix problem with multiple groups
        public override async Task<string> Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            string response = string.Empty;

            var parameters = message.Text.Split(' ');

            if(parameters.Length > 1)
            {
                long group;
                if(long.TryParse(parameters[1], out group))
                {
                    response += await AddGroupToResponse(group);
                }

                return (await client.SendTextMessageAsync(chatId,
                response)).Text;
            }
            else
            {
                var account = await _accountService
                .GetAccountByTelegramId(chatId);

                if (account.Role.Is(Core.Entities.UserRole.Student))
                {
                    var student = await _studentService
                        .GetStudentByAccountIdAsync(account.Id);
                    var groups = (await _studentService
                        .GetStudentStudyGroupsByStudentIdAsync(student.Data.Id))
                        .Data;

                    if (groups.Count == 1)
                    {
                        response += await AddGroupToResponse(groups[0].Id);
                    }
                    else if (groups.Count > 1)
                    {
                        response += "Choose student group:";
                        List<List<InlineKeyboardButton>> keyboard = new List<List<InlineKeyboardButton>>();
                        foreach (var group in groups)
                        {
                            keyboard.Add(
                                new List<InlineKeyboardButton>()
                                {
                                InlineKeyboardButton.WithCallbackData(
                                    group.Name,
                                    "/" + Name + " " + group.Id.ToString())
                                });
                        }
                        var inlineKeyboard = new InlineKeyboardMarkup(keyboard);
                        return (await client.SendTextMessageAsync(chatId, response, replyMarkup: inlineKeyboard)).Text;
                    }
                }
            }

            if(response == string.Empty)
            {
                response += "You don't have any classmates.\n";
            }

            return (await client.SendTextMessageAsync(chatId,
                response)).Text;
        }

        private async Task<string> AddGroupToResponse(long groupId)
        {
            string result = string.Empty;
            var group = (await _studentGroupService
                        .GetStudentGroupByIdAsync(groupId)).Data;
            result += group.Name + "\n\n";

            foreach (var studentId in group.StudentIds)
            {
                var otherStudent = (await _studentService
                    .GetStudentByIdAsync(studentId)).Data;
                result += otherStudent.FirstName + " "
                    + otherStudent.LastName + " "
                    + otherStudent.Email + "\n";
            }

            return result;
        }
    }
}
