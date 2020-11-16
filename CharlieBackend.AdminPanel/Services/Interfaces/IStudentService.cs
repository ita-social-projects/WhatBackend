using CharlieBackend.AdminPanel.Models.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IList<StudentViewModel>> GetAllStudentsAsync(string accessToken);
    }
}
