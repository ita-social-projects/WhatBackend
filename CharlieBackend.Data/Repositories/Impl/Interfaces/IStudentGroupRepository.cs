using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Student;
using CharlieBackend.Core.Models.StudentGroup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
	public interface IStudentGroupRepository : IRepository<StudentGroup>
	{
		public new Task<List<StudentGroup>> GetAllAsync();
		public Task<bool> IsGroupNameTakenAsync(string name);
		public StudentGroup SearchStudentGroup(long studentGroupId);
		public bool DeleteStudentGroup(long StudentGroupModelId);
		
	}
}

