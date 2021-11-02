using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Panel.Models.Students;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IList<StudentViewModel>> GetAllStudentsAsync();

        Task<StudentEditViewModel> GetStudentByIdAsync(long id);

        Task<UpdateStudentDto> UpdateStudentAsync(long id, UpdateStudentDto UpdateDto);

        Task<StudentDto> AddStudentAsync(long id);

        Task<bool> DisableStudentAsync(long id);

        Task<bool> EnableStudentAsync(long id);
    }
}
