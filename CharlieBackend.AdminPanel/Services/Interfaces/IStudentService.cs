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
        Task<IList<StudentViewModel>> GetAllStudentsAsync();

        Task<StudentEditViewModel> GetStudentByIdAsync(long id);

        Task<UpdateStudentDto> UpdateStudentAsync(long id, UpdateStudentDto UpdateDto);

        Task<StudentDto> AddStudentAsync(long id);

        Task<StudentDto> DisableStudentAsync(long id);
    }
}
