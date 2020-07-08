using CharlieBackend.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CharlieBackend.Core.Models.Student;

namespace CharlieBackend.Business.Services.Interfaces
{
	public interface IStudentService
	{
		public Task<StudentModel> CreateStudentAsync(StudentModel studentModel);
		public Task<List<StudentModel>> GetAllStudentsAsync();
		public Task<StudentModel> UpdateStudentAsync(StudentModel mentorModel);
	}
}
