using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Panel.Models.Classbook;
using System;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    /// <summary>
    /// Interface that describes methods for work with classbook and its data.
    /// </summary>
    public interface IClassbookService
    {
        /// <summary>
        /// Method for obtaining classbook-related data.
        /// </summary>
        /// <param name="studentsRequestDto">DTO with an optional set of 
        /// filters for make up classbook.</param>
        /// <returns>model with data required for viewing a classbook.</returns>
        Task<ClassbookViewModel> GetClassbookAsync(StudentsRequestDto<ClassbookResultType> studentsRequestDto);
    }
}
