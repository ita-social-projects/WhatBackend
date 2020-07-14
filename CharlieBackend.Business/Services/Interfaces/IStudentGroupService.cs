using CharlieBackend.Core.Models.StudentGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentGroupService
    {
        public Task<StudentGroupModel> CreateStudentGroupAsync(CreateStudentGroupModel studentGroupModel);
        public Task<List<StudentGroupModel>> GetAllStudentGroupsAsync();
        public Task<bool> IsGroupNameTakenAsync(string groupName);
        public bool DeleteStudentGrop(long StudentGroupId);
        public Task<StudentGroupModel> UpdateStudentGroupAsync(UpdateStudentGroupModel studentGroupModel);
        public Task<StudentGroupById> GetStudentGroupByIdAsync(long id);
    }
}
