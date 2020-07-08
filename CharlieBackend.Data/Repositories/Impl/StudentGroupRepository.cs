using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
	public class StudentGroupRepository : Repository<StudentGroup>, IStudentGroupRepository
	{
		public StudentGroupRepository(ApplicationContext applicationContext) : base(applicationContext){ }

		public new Task<List<StudentGroup>> GetAllAsync()
		{
			return _applicationContext.StudentGroups.ToListAsync();
		}


	}
}
