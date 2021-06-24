using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Schedule;
using Microsoft.Extensions.Options;
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
        private readonly ScheduleApiEndpoints _scheduleApiEndpoints;

        public ScheduleService(IApiUtil apiUtil, IOptions<ApplicationSettings> options)
        {
            _apiUtil = apiUtil;
            _scheduleApiEndpoints = options.Value.Urls.ApiEndpoints.Schedule;
        }

        public async Task<IList<EventOccurrenceDTO>> GetAllEventOccurrences()
        {
            var result = await _apiUtil
                .GetAsync<IList<EventOccurrenceDTO>>(_scheduleApiEndpoints.EventOccurrencesEndpoint);

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
                url: _scheduleApiEndpoints.EventsEndpoint,
                data: scheduledEventFilterDto);

            return result;
        }

        public async Task<EventOccurrenceDTO> GetEventOccurrenceById(long id)
        {
            var eventOccurence =
               string.Format(_scheduleApiEndpoints.EventOccurrenceById, id);

            var result = await _apiUtil
                .GetAsync<EventOccurrenceDTO>(eventOccurence);

            return result;
        }
    }
}
