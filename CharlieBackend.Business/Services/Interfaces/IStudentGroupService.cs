using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentGroupService
    {
        Task<Result<StudentGroupDto>> CreateStudentGroupAsync(CreateStudentGroupDto studentGroupModel);

        Task<Result<IList<StudentGroupDto>>> GetAllStudentGroupsAsync();

        Task<Result<bool>> IsGroupNameExistAsync(string groupName);

        bool DeleteStudentGrop(long StudentGroupId);

        Task<Result<StudentGroupDto>> UpdateStudentGroupAsync(long id, UpdateStudentGroupDto studentGroupModel);

        Task<Result<StudentGroupDto>> GetStudentGroupByIdAsync(long id);

        void AddStudentOfStudentGroups(IEnumerable<StudentOfStudentGroup> items);
    }
}
