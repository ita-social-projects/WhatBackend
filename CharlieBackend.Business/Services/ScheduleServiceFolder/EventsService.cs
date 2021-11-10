using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CharlieBackend.Business.Exceptions;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Business.Services.Notification.Interfaces;
using CharlieBackend.Business.Services.ScheduleServiceFolder.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public class EventsService : IEventsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHangfireJobService _jobService;

        public EventsService(IUnitOfWork unitOfWork, IMapper mapper, IHangfireJobService jobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jobService = jobService;
        }

        public async Task<Result<bool>> DeleteAsync(long id)
        {
            ScheduledEvent scheduledEvent = await _unitOfWork.ScheduledEventRepository.GetByIdAsync(id);

            await _unitOfWork.ScheduledEventRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            await _jobService.DeleteScheduledEventsJob(new List<ScheduledEvent>{ scheduledEvent });

            return Result<bool>.GetSuccess(true);
        }

        public async Task<ScheduledEventDTO> UpdateAsync(long id, UpdateScheduledEventDto updatedSchedule)
        {
            string errorMsg = await ValidateUpdatedScheduleAsync(updatedSchedule);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                throw new EntityValidationException(errorMsg);
            }

            ScheduledEvent schedule = await _unitOfWork.ScheduledEventRepository.GetByIdAsync(id);

            schedule = SchedulesUpdater.UpdateFields(schedule, updatedSchedule);
            _unitOfWork.ScheduledEventRepository.Update(schedule);
            await _unitOfWork.CommitAsync();

            await _jobService.CreateUpdateScheduledEventsJob(new List<ScheduledEvent> { schedule });

            return _mapper.Map<ScheduledEventDTO>(schedule);
        }

        public async Task<ScheduledEventDTO> GetAsync(long id)
        {
            return _mapper.Map<ScheduledEventDTO>(await _unitOfWork.ScheduledEventRepository.GetByIdAsync(id));
        }

        private async Task<string> ValidateUpdatedScheduleAsync(UpdateScheduledEventDto updatedSchedule)
        {
            if (await _unitOfWork.MentorRepository.GetByIdAsync(updatedSchedule.MentorId.GetValueOrDefault()) is null)
            {
               return ExceptionsConstants.MentorNotValid;
            }
            if (await _unitOfWork.ThemeRepository.GetByIdAsync(updatedSchedule.ThemeId.GetValueOrDefault()) is null)
            {
                return ExceptionsConstants.ThemeNotValid;
            }
            if (await _unitOfWork.StudentGroupRepository.GetByIdAsync(updatedSchedule.StudentGroupId.GetValueOrDefault()) is null)
            {
                return ExceptionsConstants.StudentGroupNotValid;
            }

            return string.Empty;
        }

        public async Task<Result<ScheduledEventDTO>> ConnectScheduleToLessonById(long eventId, long lessonId)
        {
            if (await _unitOfWork.ScheduledEventRepository.IsLessonConnectedToSheduledEventAsync(lessonId))
            {
                return Result<ScheduledEventDTO>.GetError(ErrorCode.Conflict, $"Lesson with id={lessonId} is already associated with another ScheduledEvent");
            }

            var scheduleEntity = await _unitOfWork.ScheduledEventRepository.ConnectEventToLessonByIdAsync(eventId, lessonId);

            return scheduleEntity == null ?
                Result<ScheduledEventDTO>.GetError(ErrorCode.NotFound, $"Scheduled event with id={eventId} does not exist") :
                Result<ScheduledEventDTO>.GetSuccess(_mapper.Map<ScheduledEventDTO>(scheduleEntity));
        }
    }
}
