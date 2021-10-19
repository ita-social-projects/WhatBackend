using CharlieBackend.Business.Models;
using CharlieBackend.Business.Services.Notification.Enums;
using CharlieBackend.Business.Services.Notification.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Notification
{
    public class HangfireNotificationService : IHangfireNotificationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly IHomeworkStudentRepository _homeworkStudentRepository;
        private readonly IStudentGroupRepository _studentGroupRepository;
        private readonly IJobMappingRepository _jobMappingRepository;
        private readonly IScheduledEventRepository _scheduledEventRepository;
        private readonly IUnitOfWork _unitOfWork;
        public HangfireNotificationService(IServiceProvider serviceProvider,
            IHomeworkRepository homeworkRepository, IHomeworkStudentRepository homeworkStudentRepository,
            IStudentGroupRepository studentGroupRepository, IJobMappingRepository jobMappingRepository,
            IScheduledEventRepository scheduledEventRepository, IUnitOfWork unitOfWork)
        {
            _jobMappingRepository = jobMappingRepository;
            _serviceProvider = serviceProvider;
            _homeworkRepository = homeworkRepository;
            _homeworkStudentRepository = homeworkStudentRepository;
            _studentGroupRepository = studentGroupRepository;
            _scheduledEventRepository = scheduledEventRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAddHomeworkNotification(long homeworkId, HomeworkInvokePeriod homeworkPeriod)
        {
            if (homeworkPeriod == HomeworkInvokePeriod.Moment)
            {
                await _jobMappingRepository.DeleteByCustomJobId(HangfireIdConverter.CreateCustomHomeworkJobId(homeworkId));
                await _unitOfWork.CommitAsync();
            }

            var homework = await _homeworkRepository.GetByIdAsync(homeworkId);

            var studentGroupId = homework.Lesson.StudentGroupId;

            var students = await _studentGroupRepository.GetGroupStudentsByGroupId(studentGroupId.Value);

            var studentsAccounts = students.Select(x => x.Account);

            string notificationMessage = $"{GetInvokeHomeworkPeriod(homeworkPeriod)} the homework:" +
                $"\n\n{homework.TaskText}\n\nGoog luck!";

            var client = await Bot.Get(_serviceProvider);

            var tasks = studentsAccounts.Where(acc => acc.TelegramId != null).Select(acc =>
            {
                return client.SendTextMessageAsync(acc.TelegramId, notificationMessage);
            });

            await Task.WhenAll(tasks);
        }

        public async Task CreateAddHomeworkStudentNotification(long homeworkStudentId)
        {
            await SendHomeworkStudentNotificationBase(homeworkStudentId, "passed");
        }

        public async Task CreateUpdateHomeworkStudentNotification(long homeworkStudentId)
        {
            await SendHomeworkStudentNotificationBase(homeworkStudentId, "passed again");
        }

        public async Task CreateUpdateMarkNotification(long homeworkStudentId)
        {
            var homeworkStudent = await _homeworkStudentRepository.GetByIdAsync(homeworkStudentId);
            var telegramId = homeworkStudent.Student.Account.TelegramId;
            if (telegramId == null)
            {
                return;
            }
            var client = await Bot.Get(_serviceProvider);
            var notificationMessage = $"You've got mark {homeworkStudent.Mark.Value},";
            if (homeworkStudent.Mark.Comment != null)
            {
                notificationMessage += $"\nwith comment {homeworkStudent.Mark.Comment},";
            }
            notificationMessage += $"\nfor the homework: \n{homeworkStudent.Homework.TaskText}";
            await client.SendTextMessageAsync(telegramId, notificationMessage);
        }

        public async Task CreateAddScheduledEventNotification(long scheduledEventId, ScheduledEventInvokePeriod scheduledEventPeriod)
        {
            if(scheduledEventPeriod == ScheduledEventInvokePeriod.Moment)
            {
                await _jobMappingRepository.DeleteByCustomJobId(HangfireIdConverter.CreateCustomScheduledEventJobId(scheduledEventId));
                await _unitOfWork.CommitAsync();
            }

            var scheduledEvent = await _scheduledEventRepository.GetByIdAsync(scheduledEventId);

            var studentsAccounts = scheduledEvent.StudentGroup.StudentsOfStudentGroups.Select(x => x.Student.Account);

            var mentorsAccounts = scheduledEvent.StudentGroup.MentorsOfStudentGroups.Select(x => x.Mentor.Account);

            string notificationMessage = $"{GetInvokeScheduledEventPeriod(scheduledEventPeriod)} with theme: \"{scheduledEvent.Theme.Name}\"";

            var client = await Bot.Get(_serviceProvider);

            var tasks = studentsAccounts.Where(acc => acc.TelegramId != null).Select(acc =>
            {
                return client.SendTextMessageAsync(acc.TelegramId, notificationMessage);
            });

            tasks.Concat(mentorsAccounts.Where(acc => acc.TelegramId != null).Select(acc =>
            {
                return client.SendTextMessageAsync(acc.TelegramId, notificationMessage);
            }));

            await Task.WhenAll(tasks);
        }

        private async Task SendHomeworkStudentNotificationBase(long homeworkStudentId, string message)
        {
            var homeworkStudent = await _homeworkStudentRepository.GetByIdAsync(homeworkStudentId);

            await _jobMappingRepository.DeleteByCustomJobId(HangfireIdConverter.CreateCustomHomeworkJobId(homeworkStudent.HomeworkId));
            await _unitOfWork.CommitAsync();

            var client = await Bot.Get(_serviceProvider);
            var studentFullName = $"{homeworkStudent.Student.Account.FirstName} {homeworkStudent.Student.Account.LastName}";
            var notificationMessage =
                $"Student {studentFullName} {message} his homework for the task:\n{homeworkStudent.Homework.TaskText}";
            var mentorsAccounts = homeworkStudent.Homework.Lesson.StudentGroup.MentorsOfStudentGroups
                .Select(x => x.Mentor.Account);

            var tasks = mentorsAccounts.Where(acc => acc.TelegramId != null).Select(acc =>
            {
                return client.SendTextMessageAsync(acc.TelegramId, notificationMessage);
            });

            await Task.WhenAll(tasks);
        }

        private static string GetInvokeHomeworkPeriod(HomeworkInvokePeriod invokePeriod)
        {
            return invokePeriod switch
            {
                HomeworkInvokePeriod.Day => "In a day the deadline will come for",
                HomeworkInvokePeriod.TwoHours => "In two hours the deadline will come for",
                HomeworkInvokePeriod.Moment => "The deadline has come for",
                HomeworkInvokePeriod.CreationMoment => "Mentor has just publish",
                _ => throw new ArgumentException($"Type of {nameof(HomeworkInvokePeriod)} is not supported")
            };
        }

        private static string GetInvokeScheduledEventPeriod(ScheduledEventInvokePeriod scheduledEventPeriod)
        {
            return scheduledEventPeriod switch
            {
                ScheduledEventInvokePeriod.FortyMinutes => "In forty minutes the event will start",
                ScheduledEventInvokePeriod.Moment => "The event has started",
                _ => throw new ArgumentException($"Type of {nameof(ScheduledEventInvokePeriod)} is not supported")
            };
        }
    }
}
