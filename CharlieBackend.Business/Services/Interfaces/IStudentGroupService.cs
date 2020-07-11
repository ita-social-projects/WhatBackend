using CharlieBackend.Core.Models.StudentGroup;
using CharlieBackend.Core.Models.Theme;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
	public interface IStudentGroupService
	{
		public Task<StudentGroupModel> CreateStudentGroupAsync(StudentGroupModel studentGroup);
		public Task<List<StudentGroupModel>> GetAllStudentGroupsAsync();
		public Task<bool> IsGroupNameTakenAsync(string email);
		public bool DeleteStudentGrop(long StudentGroupId);
		public bool UpdateStudentGroup(StudentGroupModel studentGroupModel);
		public Task<StudentGroupModel> SearchStudentGroup(long studentGroupId);
	}
}
