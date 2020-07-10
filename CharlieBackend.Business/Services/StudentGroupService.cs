using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Student;
using CharlieBackend.Core.Models.StudentGroup;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

		public async Task<StudentGroupModel> CreateStudentGroupAsync(StudentGroupModel studentGroup)
		{
			
				_unitOfWork.StudentGroupRepository.Add(new StudentGroup
				{
					Name = studentGroup.name,
					StartDate = Convert.ToDateTime(studentGroup.start_date.ToString()),
					FinishDate = Convert.ToDateTime(studentGroup.finish_date.ToString()),
				});

				await _unitOfWork.CommitAsync();
				return studentGroup;
			
		}

		public Task<bool> IsGroupNameTakenAsync(string name)
		{
			return _unitOfWork.StudentGroupRepository.IsGroupNameTakenAsync(name);
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
