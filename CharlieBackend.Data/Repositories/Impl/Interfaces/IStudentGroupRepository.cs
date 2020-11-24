using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IStudentGroupRepository : IRepository<StudentGroup>
    {
        new Task<List<StudentGroup>> GetAllAsync();

        Task<List<StudentStudyGroupsDto>> GetStudentStudyGroups(long id);

        Task<bool> IsGroupNameExistAsync(string name);

        StudentGroup SearchStudentGroup(long studentGroupId);

        bool DeleteStudentGroup(long StudentGroupModelId);

        void UpdateManyToMany(IEnumerable<StudentOfStudentGroup> currentStudentsOfStudentGroup,
                                   IEnumerable<StudentOfStudentGroup> newStudentsOfStudentGroup);
    }
}

