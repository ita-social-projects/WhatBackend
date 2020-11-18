using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentGroupService
    {
        public Task<Result<StudentGroupDto>> CreateStudentGroupAsync(CreateStudentGroupDto studentGroupModel);

        public Task<IList<StudentGroupDto>> GetAllStudentGroupsAsync();

        public Task<Result<bool>> IsGroupNameExistAsync(string groupName);

        public bool DeleteStudentGrop(long StudentGroupId);

        public Task<Result<UpdateStudentGroupDto>> UpdateStudentGroupAsync(long id, UpdateStudentGroupDto studentGroupModel);

        public Task<Result<UpdateStudentsForStudentGroup>> UpdateStudentsForStudentGroupAsync(long id, UpdateStudentsForStudentGroup studentGroupModel);

        public Task<Result<StudentGroupDto>> GetStudentGroupByIdAsync(long id);
    }
}
