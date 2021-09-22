using CharlieBackend.Panel.Models.Calendar;
using CharlieBackend.Core.DTO.Schedule;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    /// <summary>
    /// Interface that describes methods for work with calendar and it's data.
    /// </summary>
    public interface ICalendarService
    {
        /// <summary>
        /// Method for obtaining calendar-related data.
        /// </summary>
        /// <param name="scheduledEventFilter">DTO with an optional set of 
        /// filters for scheduled events.</param>
        /// <returns>model with data required for viewing a calendar.</returns>
        Task<CalendarViewModel> GetCalendarDataAsync(ScheduledEventFilterRequestDTO scheduledEventFilter);
    }
}
