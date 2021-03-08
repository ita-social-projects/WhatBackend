﻿using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Models.ResultModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IScheduleService
    {
        public Task<Result<EventOccurrenceDTO>> CreateScheduleAsync(CreateScheduleDto scheduleModel);

        public Task<Result<EventOccurrenceDTO>> GetEventOccurrenceByIdAsync(long id);

        public Task<Result<ScheduledEventDTO>> UpdateScheduledEventByID(long scheduledEvendID, UpdateScheduledEventDto scheduleModel);

        public Task<Result<IList<ScheduledEventDTO>>> UpdateEventsRange(ScheduledEventFilterRequestDTO filter, UpdateScheduledEventDto request);

        public Task<Result<IList<ScheduledEventDTO>>> GetEventsFiltered(ScheduledEventFilterRequestDTO request);

        public Task<Result<EventOccurrenceDTO>> DeleteScheduleByIdAsync(long studentGroupId, DateTime? startDate, DateTime? finishDate);

        public Task<Result<bool>> DeleteConcreteScheduleByIdAsync(long eventId);

        public Task<Result<EventOccurrenceDTO>> UpdateEventOccurrenceById(long eventOccurrenceId, CreateScheduleDto request);
    }
}
