using CharlieBackend.Business.Services.Notification.Enums;
using CharlieBackend.Business.Services.Notification.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Notification
{
    public class HangfireJobService : IHangfireJobService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HangfireJobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAddHomeworkJob(Homework homework)
        {
            DateTime dueDate = homework.DueDate ?? default;

            if (homework.DueDate != default)
            {
                BackgroundJob.Enqueue<IHangfireNotificationService>(x => x.CreateAddHomeworkNotification(homework.Id,
                    HomeworkInvokePeriod.CreationMoment));

                var executeDate = dueDate.Subtract(new TimeSpan(1, 0, 0, 0));

                CreateAddHomeworkJobBase(homework, HomeworkInvokePeriod.Day, executeDate);

                executeDate = dueDate.Subtract(new TimeSpan(0, 2, 0, 0));

                CreateAddHomeworkJobBase(homework, HomeworkInvokePeriod.TwoHours, executeDate);

                executeDate = dueDate.Subtract(new TimeSpan(0, 0, 0, 15));

                CreateAddHomeworkJobBase(homework, HomeworkInvokePeriod.Moment, executeDate);

                await _unitOfWork.CommitAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> CreateUpdateHomeworkJob(Homework homework)
        {
            var customJobId = HangfireIdConverter.CreateCustomHomeworkJobId(homework.Id);
            var hangfireJobIds = await _unitOfWork.JobMappingRepository
                .GetHangfireIdsByCustomJobId(customJobId);

            foreach (var jobId in hangfireJobIds)
            {
                BackgroundJob.Delete(jobId);
            }

            await _unitOfWork.JobMappingRepository.DeleteByCustomJobId(customJobId);

            return await CreateAddHomeworkJob(homework);
        }

        public async Task<bool> CreateAddHomeworkStudentJob(HomeworkStudent homeworkStudent)
        {
            var customJobId = HangfireIdConverter.CreateCustomHomeworkJobId(homeworkStudent.HomeworkId);

            var hangfireJobIds = await _unitOfWork.JobMappingRepository.GetHangfireIdsByCustomJobId(customJobId);

            foreach (var Id in hangfireJobIds)
            {
                BackgroundJob.Delete(Id);
            }

            var jobId = BackgroundJob.Enqueue<IHangfireNotificationService>(x => x.CreateAddHomeworkStudentNotification(homeworkStudent.Id));

            return jobId != null ? true : false;
        }

        public async Task<bool> CreateUpdateHomeworkStudentJob(HomeworkStudent homeworkStudent)
        {
            var jobId = BackgroundJob.Enqueue<IHangfireNotificationService>(x => x.CreateUpdateHomeworkStudentNotification(homeworkStudent.Id));

            return jobId != null ? true : false;
        }

        public async Task<bool> CreateUpdateMarkJob(HomeworkStudent homeworkStudent)
        {
            var customJobId = HangfireIdConverter.CreateCustomHomeworkJobId(homeworkStudent.HomeworkId);

            var hangfireJobIds = await _unitOfWork.JobMappingRepository.GetHangfireIdsByCustomJobId(customJobId);

            foreach (var Id in hangfireJobIds)
            {
                BackgroundJob.Delete(Id);
            }

            var jobId = BackgroundJob.Enqueue<IHangfireNotificationService>(x => x.CreateUpdateMarkNotification(homeworkStudent.Id));

            return jobId != null ? true : false;
        }

        public async Task<bool> CreateAddScheduledEventsJob(IEnumerable<ScheduledEvent> scheduledEvents)
        {
            foreach (var scheduledEvent in scheduledEvents)
            {
                var executeDate = scheduledEvent.EventStart.Subtract(new TimeSpan(0, 0, 40, 0));
                CreateAddEventJobBase(scheduledEvent, ScheduledEventInvokePeriod.FortyMinutes, executeDate);

                executeDate = scheduledEvent.EventStart.Subtract(new TimeSpan(0, 0, 0, 15));
                CreateAddEventJobBase(scheduledEvent, ScheduledEventInvokePeriod.Moment, executeDate);
            }

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> CreateUpdateScheduledEventsJob(IEnumerable<ScheduledEvent> scheduledEvents)
        {
            await DeleteScheduledEventsJobBase(scheduledEvents);

            return await CreateAddScheduledEventsJob(scheduledEvents);
        }

        public async Task<bool> CreateUpdateEventOccurrenceJob(IEnumerable<ScheduledEvent> removedScheduledEvents,
            IEnumerable<ScheduledEvent> scheduledEvents)
        {
            await DeleteScheduledEventsJobBase(removedScheduledEvents);

            return await CreateAddScheduledEventsJob(scheduledEvents);
        }

        public async Task<bool> DeleteScheduledEventsJob(IEnumerable<ScheduledEvent> scheduledEvents)
        {
            if (scheduledEvents.Count() != 0)
            {
                await DeleteScheduledEventsJobBase(scheduledEvents);

                await _unitOfWork.CommitAsync();
            }

            return true;
        }

        private void CreateAddEventJobBase(ScheduledEvent scheduledEvent, ScheduledEventInvokePeriod invokePeriod, DateTime executeDate)
        {
            var customJobId = HangfireIdConverter.CreateCustomScheduledEventJobId(scheduledEvent.Id);
            if (executeDate >= DateTime.Now)
            {
                var jobId = BackgroundJob.Schedule<IHangfireNotificationService>(x => x.CreateAddScheduledEventNotification(scheduledEvent.Id, invokePeriod),
                    new DateTimeOffset(executeDate));

                _unitOfWork.JobMappingRepository.Add(new JobMapping()
                {
                    CustomJobID = customJobId,
                    HangfireJobID = jobId
                });
            }
        }

        private void CreateAddHomeworkJobBase(Homework homework, HomeworkInvokePeriod invokePeriod, DateTime executeDate)
        {
            var customJobId = HangfireIdConverter.CreateCustomHomeworkJobId(homework.Id);
            if (executeDate >= DateTime.Now)
            {
                var jobId = BackgroundJob.Schedule<IHangfireNotificationService>(x => x.CreateAddHomeworkNotification(homework.Id, invokePeriod),
                    new DateTimeOffset(executeDate));

                _unitOfWork.JobMappingRepository.Add(new JobMapping()
                {
                    CustomJobID = customJobId,
                    HangfireJobID = jobId
                });
            }
        }

        private async Task DeleteScheduledEventsJobBase(IEnumerable<ScheduledEvent> scheduledEvents)
        {
            foreach (var scheduledEvent in scheduledEvents)
            {
                var customJobId = HangfireIdConverter.CreateCustomScheduledEventJobId(scheduledEvent.Id);
                var hangfireJobIds = await _unitOfWork.JobMappingRepository.GetHangfireIdsByCustomJobId(customJobId);

                foreach (var jobId in hangfireJobIds)
                {
                    BackgroundJob.Delete(jobId);
                }

                await _unitOfWork.JobMappingRepository.DeleteByCustomJobId(customJobId);
            }
        }
    }
}
