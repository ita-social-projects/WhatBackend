using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentGroupService
    {
        public Task<StudentGroupDto> CreateStudentGroupAsync(CreateStudentGroupDto studentGroupModel);

        public Task<IList<StudentGroupDto>> GetAllStudentGroupsAsync();

        public Task<bool> IsGroupNameTakenAsync(string groupName);

        public bool DeleteStudentGrop(long StudentGroupId);

        public Task<UpdateStudentGroupDto> UpdateStudentGroupAsync(long id, UpdateStudentGroupDto studentGroupModel);

        public Task<UpdateStudentsForStudentGroup> UpdateStudentsForStudentGroupAsync(long id, UpdateStudentsForStudentGroup studentGroupModel);

        public Task<StudentGroupDto> GetStudentGroupByIdAsync(long id);
    }
}
