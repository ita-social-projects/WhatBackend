using CharlieBackend.Core.DTO.Event;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Panel.Models.EventOccurrence;
using CharlieBackend.Panel.Models.ScheduledEvent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    /// <summary>
    /// Interface that describes accessible methods to work with schedules.
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>
        /// Method for obtaining a collection of scheduled events
        /// using given filter DTO.
        /// </summary>
        /// <param name="scheduledEventFilterDto">
        /// DTO with a set of optional filtration parameters.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>filtered collection of scheduled events.</returns>
        Task<IList<ScheduledEventDTO>> GetEventsFiltered(
            ScheduledEventFilterRequestDTO scheduledEventFilterDto);

        /// <summary>
        /// Method for obtaining a collection of all event occurrences.
        /// </summary>
        /// <returns>a collection of all event occurrences.</returns>
        Task<IList<EventOccurrenceDTO>> GetAllEventOccurrences();

        /// <summary>
        /// Method for obtaining a specified event occurrence by id.
        /// </summary>
        /// <returns>EventOccurrenceDTO with specified id</returns>
        Task<EventOccurrenceDTO> GetEventOccurrenceById(long id);

        /// <summary>
        /// Method for adding new event occurrence.
        /// </summary>
        Task CreateScheduleAsync(CreateScheduleDto scheduleDTO);

        /// <summary>
        /// Method for updating event occurrence by id.
        /// </summary>
        Task UpdateScheduleByIdAsync(long eventOccurrenceID, CreateScheduleDto updateScheduleDto);

        /// <summary>
        /// Method for deleting event occurrence by id.
        /// </summary>
        Task DeleteScheduleByIdAsync(long eventOccurrenceID);

        /// <summary>
        /// Method for getting required lists to add new event occurrence.
        /// </summary>
        Task<EventOccurrenceEditViewModel> PrepareEventAddAsync();
        
        /// <summary>
        /// Method for getting required lists to update single event.
        /// </summary>
        Task<ScheduledEventEditViewModel> PrepareSingleEventUpdateAsync(long id);

        /// <summary>
        /// Method for updating single event by id.
        /// </summary>
        Task UpdateSingleEventByIdAsync(long id, UpdateScheduledEventDto updatedSchedule);
        
        /// <summary>
        /// Method for getting required lists to update event occurrence.
        /// </summary>
        Task<EventOccurrenceEditViewModel> PrepareEventOcccurrenceUpdateAsync(long id);

        /// <summary>
        ///  Method for adding new single event.
        /// </summary>
        Task CreateSingleEventAsync(CreateSingleEventDto singleEventDTO);

        /// <summary>
        /// Method for deleting single event by id.
        /// </summary>
        Task DeleteSingleEventByIdAsync(long eventID);
    }
}
