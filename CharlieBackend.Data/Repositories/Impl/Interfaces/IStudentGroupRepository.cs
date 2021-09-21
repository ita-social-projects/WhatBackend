using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IStudentGroupRepository : IRepository<StudentGroup>
    {
        new Task<List<StudentGroup>> GetAllAsync();

        Task<IList<StudentGroup>> GetAllActiveAsync(DateTime? startDate, DateTime? finishDate);

        Task<StudentGroup> GetActiveStudentGroupByIdAsync(long id);

        Task<List<StudentStudyGroupsDto>> GetStudentStudyGroups(long id);

        Task<List<MentorStudyGroupsDto>> GetMentorStudyGroups(long id);

        Task<IList<long?>> GetGroupStudentsIds(long id);

        Task<bool> IsGroupOnCourseAsync(long id);

        Task<bool> IsGroupNameExistAsync(string name);

        new Task<StudentGroup> GetByIdAsync(long id);

        void AddStudentOfStudentGroups(IEnumerable<StudentOfStudentGroup> items);

        Task<bool> DeleteStudentGroupAsync(long StudentGroupModelId);

        Task<bool> DeactivateStudentGroupAsync(long StudentGroupModelId);

        public void UpdateManyToMany(IEnumerable<StudentOfStudentGroup> currentStudentsOfStudentGroup,
                                     IEnumerable<StudentOfStudentGroup> newStudentsOfStudentGroup);

        Task<IList<StudentGroup>> GetStudentGroupsByDateAsync(DateTime? startDate, DateTime? finishDate);
        
        Task<IList<long?>> GetStudentGroupsIdsByStudentId(long id);
    }
}
