using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Panel.Models.GroupsRegister;
using System;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    /// <summary>
    /// Interface that describes methods for work with calendar and it's data.
    /// </summary>
    public interface IGroupsRegisterService
    {
        /// <summary>
        /// Method for obtaining calendar-related data.
        /// </summary>
        /// <param name="scheduledEventFilter">DTO with an optional set of 
        /// filters for scheduled events.</param>
        /// <returns>model with data required for viewing a calendar.</returns>
        Task<FilteredRegisterViewModel> GetRegisterAsync(StudentsRequestDto<Enum> groupsRegisterFilter);
    }
}
