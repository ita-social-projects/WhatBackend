using CharlieBackend.AdminPanel.Models.StudentGroups;
using CharlieBackend.Core.DTO.StudentGroups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface IStudentGroupService
    {
        Task<IList<StudentGroupViewModel>> GetAllStudentGroupsAsync(string accessToken);

        Task<StudentGroupEditViewModel> PrepareStudentGroupUpdateAsync(long id, string accessToken);

        Task<StudentGroupEditViewModel> PrepareStudentGroupAddAsync(string accessToken);

        Task<StudentGroupDto> UpdateStudentGroupAsync(long id, StudentGroupDto updateDto, string accessToken);

        Task<CreateStudentGroupDto> AddStudentGroupAsync(long id, CreateStudentGroupDto addDto, string accessToken);
    }
}
