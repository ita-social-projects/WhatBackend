using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.StudentGroup;
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

        public Task<StudentGroup> UpdateStudentGroupAsync(UpdateStudentGroupModel studentGroupModel);

        public Task<StudentGroupById> GetStudentGroupByIdAsync(long id);
    }
}
