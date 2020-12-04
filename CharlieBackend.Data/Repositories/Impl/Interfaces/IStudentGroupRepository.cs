using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IStudentGroupRepository : IRepository<StudentGroup>
    {
        public new Task<List<StudentGroup>> GetAllAsync();

        public Task<bool> IsGroupNameExistAsync(string name);

        public StudentGroup SearchStudentGroup(long studentGroupId);

        public bool DeleteStudentGroup(long StudentGroupModelId);

        public void UpdateManyToMany(IEnumerable<StudentOfStudentGroup> currentStudentsOfStudentGroup,
                                    IEnumerable<StudentOfStudentGroup> newStudentsOfStudentGroup);

        void AddStudentOfStudentGroups(IEnumerable<StudentOfStudentGroup> items);
    }
}

