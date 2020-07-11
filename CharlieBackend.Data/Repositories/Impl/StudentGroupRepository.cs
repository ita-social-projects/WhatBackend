using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Student;
using CharlieBackend.Core.Models.StudentGroup;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
	public class StudentGroupRepository : Repository<StudentGroup>, IStudentGroupRepository
	{
		public StudentGroupRepository(ApplicationContext applicationContext) : base(applicationContext){ }

		public bool DeleteStudentGroup(long StudentGroupModelId)
		{
			var x = SearchStudentGroup(StudentGroupModelId);
			if (x == null)
				return false;
			else
			{
				_applicationContext.StudentGroups.Remove(x);
				_applicationContext.SaveChanges();
				return true;
			}
		}

		public new Task<List<StudentGroup>> GetAllAsync()
		{
			return _applicationContext.StudentGroups.ToListAsync();
		}

		public Task<bool> IsGroupNameTakenAsync(string name)
		{
			return _applicationContext.StudentGroups.AnyAsync(grName => grName.Name == name);
		}

		public StudentGroup SearchStudentGroup(long studentGroupId)
		{
			foreach(var x in _applicationContext.StudentGroups) {
				if (x.Id == studentGroupId) return x;
			}
			return null;
		}
	}
}
