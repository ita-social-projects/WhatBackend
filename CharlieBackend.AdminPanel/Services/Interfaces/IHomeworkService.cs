using CharlieBackend.AdminPanel.Models.Homework;
using CharlieBackend.Core.DTO.Homework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface IHomeworkService
    {
        Task<HomeworkViewModel> GetHomeworkById(long id);
        Task AddHomeworkEndpoint(HomeworkDto homeworkDto);
        Task UpdateHomeworkEndpoint(long id, HomeworkDto homeworkDto);


    }
}
