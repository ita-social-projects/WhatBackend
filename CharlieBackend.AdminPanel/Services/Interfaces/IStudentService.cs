using CharlieBackend.AdminPanel.Models.Students;
using CharlieBackend.Core.DTO.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IList<StudentViewModel>> GetAllStudentsAsync(string accessToken);

        Task<StudentEditViewModel> GetStudentByIdAsync(long id, string accessToken);

        Task<UpdateStudentDto> UpdateStudentAsync(long id, UpdateStudentDto UpdateDto, string accessToken);

        Task<StudentDto> AddStudentAsync(long id, string accessToken);
    }
}
