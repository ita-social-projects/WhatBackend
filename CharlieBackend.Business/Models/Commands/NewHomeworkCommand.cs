using CharlieBackend.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using CharlieBackend.Core.DTO.Homework;
using System.Linq;
using CharlieBackend.Business.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using CharlieBackend.Business.Helpers;

namespace CharlieBackend.Business.Models.Commands
{
    public class NewHomeworkCommand : Command
    {
        private readonly IAccountService _accountService;
        private readonly IMentorService _mentorService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IStudentService _studentService;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICourseService _courseService;
        private readonly ILessonService _lessonService;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeworkService> _loggerHomeworkService;
        private readonly ILogger<HomeworkStudentService> _loggerHomeworkStudentService;

        private IHomeworkService _homeworkService;
        private IHomeworkStudentService _homeworkStudentService;

        public override string Name => "/newhomework";

        public NewHomeworkCommand(IAccountService accountService,
            IMentorService mentorService,
            IStudentGroupService studentGroupService,
            IStudentService studentService,
            IUnitOfWork unitOfWork,
            ICourseService courseService,
            ILessonService lessonService,
            IMapper mapper,
            ILogger<HomeworkService> loggerHomeworkService,
            ILogger<HomeworkStudentService> loggerHomeworkStudentService)
        {
            _accountService = accountService;
            _mentorService = mentorService;
            _studentGroupService = studentGroupService;
            _studentService = studentService;
            _unitOfWork = unitOfWork;
            _courseService = courseService;
            _lessonService = lessonService;
            _mapper = mapper;
            _loggerHomeworkService = loggerHomeworkService;
            _loggerHomeworkStudentService = loggerHomeworkStudentService;
        }

        public override async Task<string> Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            string response = string.Empty;

            var account = await _accountService
                .GetAccountByTelegramId(chatId);
            var mentor = await _mentorService
                .GetMentorByAccountIdAsync(account.Id);

            var parameters = message.Text.Split(' ');

            if(parameters.Length > 1)
            {
                long groupId;
                if (long.TryParse(parameters[1], out groupId))
                {
                    var httpContext = new DefaultHttpContext();
                    var identity = (ClaimsIdentity)httpContext.User.Identity;
                    identity.AddClaim(new Claim(
                        ClaimsIdentity.DefaultRoleClaimType, "Mentor"));
                    identity.AddClaim(new Claim(
                        ClaimConstants.IdClaim, mentor.Data.Id.ToString()));
                    identity.AddClaim(new Claim(
                        ClaimConstants.AccountClaim, account.Id.ToString()));
                    var httpContextAccessor = new HttpContextAccessor()
                    {
                        HttpContext = httpContext
                    };
                    var currentUserService = new CurrentUserService(httpContextAccessor);
                    _homeworkService = new HomeworkService(_unitOfWork,
                        _courseService,
                        _lessonService,
                        _mapper,
                        _loggerHomeworkService,
                        currentUserService);
                    _homeworkStudentService = new HomeworkStudentService(
                        _unitOfWork,
                        _mapper,
                        _loggerHomeworkStudentService,
                        currentUserService);

                    var group = await _studentGroupService
                    .GetStudentGroupByIdAsync(groupId);
                    var homeworkTasks = await _homeworkService
                        .GetHomeworksAsync(
                        new GetHomeworkRequestDto()
                        {
                            CourseId = group.Data.CourseId,
                            StartDate = group.Data.StartDate,
                            FinishDate = group.Data.FinishDate
                        });
                    foreach (var task in homeworkTasks.Data)
                    {
                        var studentHomeworks = (await _homeworkStudentService
                            .GetHomeworkStudentForMentor(task.Id))
                            .Where(homework => homework.Mark == null)
                            .GroupBy(homework => homework.StudentId);
                        foreach (var studentHomework in studentHomeworks)
                        {
                            var student = (await _studentService
                                .GetStudentByIdAsync(studentHomework.Key)).Data;
                            response += student.FirstName + " "
                                + student.LastName + " "
                                + "__" + student.Email + "__" + "\n\n";
                            foreach (var homework in studentHomework.ToList())
                            {
                                response += homework.PublishingDate;
                                response += "\n";
                            }
                            response += "\n";
                        }
                    }
                }
            }
            else
            {
                var groups = await _mentorService
                    .GetMentorStudyGroupsByMentorIdAsync(mentor.Data.Id);

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

                return (await client.SendTextMessageAsync(chatId, response, 
                    replyMarkup: inlineKeyboard)).Text;
            }

            return (await client.SendTextMessageAsync(chatId,
                response)).Text;
        }
    }
}
