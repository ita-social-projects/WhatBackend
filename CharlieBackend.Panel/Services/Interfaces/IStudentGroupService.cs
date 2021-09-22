using CharlieBackend.Panel.Models.StudentGroups;
using CharlieBackend.Core.DTO.StudentGroups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IStudentGroupService
    {
        Task<IList<StudentGroupViewModel>> GetAllStudentGroupsAsync();

        Task<StudentGroupEditViewModel> PrepareStudentGroupUpdateAsync(long id);

        Task<StudentGroupEditViewModel> PrepareStudentGroupAddAsync();

        Task<StudentGroupDto> UpdateStudentGroupAsync(long id, StudentGroupDto updateDto);

        Task<CreateStudentGroupDto> AddStudentGroupAsync(long id, CreateStudentGroupDto addDto);
    }
}
