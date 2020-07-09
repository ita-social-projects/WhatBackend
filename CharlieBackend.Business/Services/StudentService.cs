using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Student;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICredentialsSenderService _credentialSender;

        public StudentService(IAccountService accountService, IUnitOfWork unitOfWork, ICredentialsSenderService credentialsSender)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _credentialSender = credentialsSender;
        }

        public async Task<StudentModel> CreateStudentAsync(StudentModel studentModel)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var generatedPassword = _accountService.GenerateSalt();
                    var account = new Account
                    {
                        Email = studentModel.Email,
                        FirstName = studentModel.FirstName,
                        LastName = studentModel.LastName,
                        Role = 1
                    };
                    account.Salt = _accountService.GenerateSalt();
                    account.Password = _accountService.HashPassword(generatedPassword, account.Salt);

                    var student = new Student { Account = account };
                    _unitOfWork.StudentRepository.Add(student);

                    await _unitOfWork.CommitAsync();

                    //var newAccount = await _accountService.CreateAccountAsync(account.ToAccountModel());

                    //var student = new Student { AccountId = newAccount.Id };
                    //_unitOfWork.StudentRepository.Add(student);

                    //await _unitOfWork.CommitAsync();


                    //await _unitOfWork.CommitAsync();

                    await _credentialSender.SendCredentialsAsync(account.Email, generatedPassword);
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

        public async Task<StudentModel> UpdateStudentAsync(StudentModel studentModel)
        {
            try
            {
                var foundStudent = await _unitOfWork.StudentRepository.GetByIdAsync(studentModel.Id);
                if (foundStudent == null) return null;

                foundStudent.Account.Email = studentModel.Email;
                foundStudent.Account.FirstName = studentModel.FirstName;
                foundStudent.Account.LastName = studentModel.LastName;
                foundStudent.Account.Salt = _accountService.GenerateSalt();
                foundStudent.Account.Password = _accountService.HashPassword(studentModel.Password, foundStudent.Account.Salt);

                await _unitOfWork.CommitAsync();
                return foundStudent.ToStudentModel();

            }
            catch { _unitOfWork.Rollback(); return null; }
        }
    }
}