using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Student;
using CharlieBackend.Core.Models.StudentGroup;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
	public class StudentGroupService : IStudentGroupService
	{
		private readonly IUnitOfWork _unitOfWork;

		public StudentGroupService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Task<StudentGroupModel> CreateStudentGroupAsync(StudentGroupModel studentGroup)
		{
			throw new NotImplementedException();
		}

		public async Task<List<StudentGroupModel>> GetAllStudentGroupsAsync()
		{
			var studentGroup = await _unitOfWork.StudentGroupRepository.GetAllAsync();

			var studentGroupModels = new List<StudentGroupModel>();

			foreach (var Group in studentGroup)
			{
				studentGroupModels.Add(Group.ToStudentGroupModel());
			}
			return studentGroupModels;
		}
	}
}
