using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Schedule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    /// <summary>
    /// Class that provides methods for work with schedules.
    /// </summary>
    class ScheduleService : IScheduleService
    {
        private readonly IApiUtil _apiUtil;

        public ScheduleService(IApiUtil apiUtil)
        {
            _apiUtil = apiUtil;
        }

        public async Task<IList<EventOccurrenceDTO>> GetAllEventOccurrences()
        {
            var result = await _apiUtil
                .GetAsync<IList<EventOccurrenceDTO>>("/api/schedules/event-occurrences");

            return result;
        }

        public async Task<IList<ScheduledEventDTO>> GetEventsFiltered(
            ScheduledEventFilterRequestDTO scheduledEventFilterDto)
        {
            if (scheduledEventFilterDto is null)
            {
                throw new ArgumentNullException();
            }

            var result = await _apiUtil.PostAsync<IList<ScheduledEventDTO>, ScheduledEventFilterRequestDTO>(
                url: "api/schedules/events",
                data: scheduledEventFilterDto);

            return result;
        }
    }
}
