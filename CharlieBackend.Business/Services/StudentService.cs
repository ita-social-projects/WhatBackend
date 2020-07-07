using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Student;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
	public class StudentService : IStudentService
	{
		private readonly IAccountService _accountService;
		private readonly IUnitOfWork _unitOfWork;

		public StudentService(IAccountService accountService, IUnitOfWork unitOfWork)
		{
			_accountService = accountService;
			_unitOfWork = unitOfWork;
		}

		public async Task<StudentModel> CreateStudentAsync(StudentModel studentModel)
		{
			using (var transaction = _unitOfWork.BeginTransaction())
			{
				try
				{
					// How to set password?
					var account = await _accountService.CreateAccountAsync(new Account
					{
						Email = studentModel.Email,
						FirstName = studentModel.FirstName,
						LastName = studentModel.LastName,
						Password = "temp",
						Role = 4
					}.ToAccountModel());

					var student = new Student { AccountId = account.Id };
					_unitOfWork.StudentRepository.Add(student);

					await _unitOfWork.CommitAsync();


					await _unitOfWork.CommitAsync();

					await transaction.CommitAsync();

					return student.ToStudentModel();
				}
				catch { transaction.Rollback(); return null; }
			}
		}

		public async Task<List<StudentModel>> GetAllStudentsAsync()
		{
			var students = await _unitOfWork.StudentRepository.GetAllAsync();

			var studentModels = new List<StudentModel>();

			foreach (var student in students)
			{
				studentModels.Add(student.ToStudentModel());
			}
			return studentModels;
		}
	}
}