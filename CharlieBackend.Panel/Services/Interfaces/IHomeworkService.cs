using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Panel.Models.Homework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IHomeworkService
    {
        Task<IList<HomeworkViewModel>> GetHomeworks();
        Task<HomeworkViewModel> GetHomeworkById(long id);
        Task AddHomeworkEndpoint(HomeworkDto homeworkDto);
        Task UpdateHomeworkEndpoint(long id, HomeworkDto homeworkDto);
    }
}
