using CharlieBackend.AdminPanel.Models.Calendar;
using CharlieBackend.Core.DTO.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<IList<ScheduledEventDTO>> GetEventsFiltered(ScheduledEventFilterRequestDTO request);

        Task<IList<EventOccurrenceDTO>> GetAllEventOccurrences();
    }
}
