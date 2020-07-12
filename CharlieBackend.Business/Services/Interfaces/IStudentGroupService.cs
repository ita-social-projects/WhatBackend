using CharlieBackend.Core.Models.StudentGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentGroupService
    {
        public Task<StudentGroupModel> CreateStudentGroupAsync(StudentGroupModel studentGroupModel);
        public Task<List<StudentGroupModel>> GetAllStudentGroupsAsync();
        public Task<bool> IsGroupNameTakenAsync(string groupName);
        public bool DeleteStudentGrop(long StudentGroupId);
        public Task<StudentGroupModel> UpdateStudentGroupAsync(StudentGroupModel studentGroupModel);
        public Task<StudentGroupModel> SearchStudentGroup(long studentGroupId);
    }
}
