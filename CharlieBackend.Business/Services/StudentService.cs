using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Student;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICredentialsSenderService _credentialSender;
        private readonly IMapper _mapper;

        public StudentService(IAccountService accountService, 
                              IUnitOfWork unitOfWork,
                              ICredentialsSenderService credentialsSender,
                              IMapper mapper)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _credentialSender = credentialsSender;
            _mapper = mapper;
        }

        public async Task<StudentModel> CreateStudentAsync(CreateStudentModel studentModel)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var generatedPassword = _accountService.GenerateSalt();

                    var accountEntity = _mapper.Map<Account>(studentModel);

                    //var account = new Account
                    //{
                    //    Email = studentModel.Email,
                    //    FirstName = studentModel.FirstName,
                    //    LastName = studentModel.LastName,
                    //    Role = 1
                    //};

                    accountEntity.Role = 1;
                    accountEntity.Salt = _accountService.GenerateSalt();
                    accountEntity.Password = _accountService.HashPassword(generatedPassword, accountEntity.Salt);

                    var newStudent = new Student 
                    {
                        Account = accountEntity
                    };

                    _unitOfWork.StudentRepository.Add(newStudent);

                    await _unitOfWork.CommitAsync();

                    var credentialMessageSent = false;

                    if (await _credentialSender.SendCredentialsAsync(accountEntity.Email, generatedPassword))
                    {
                        await transaction.CommitAsync();

                        credentialMessageSent = true;

                        return _mapper.Map<StudentModel>(newStudent);
                    }
                    else
                    {
                        //Have to implement here sending error message or details to calling method/controller
                        //throw new Exception("Email has not been sent");

                        await transaction.CommitAsync();

                        credentialMessageSent = false;

                        return _mapper.Map<StudentModel>(newStudent);
                    }

                }
                catch
                {
                    //Have to implement here sending error message or details to calling method/controller
                    transaction.Rollback();

                    return null;
                }
            }
        }

        public async Task<IList<StudentModel>> GetAllStudentsAsync() // feature or bug?
        {
            var students = await _unitOfWork.StudentRepository.GetAllAsync();

            var studentModels = new List<StudentModel>();

            foreach (var student in students)
            {
                studentModels.Add(_mapper.Map<StudentModel>(student));
            }

            return studentModels;
        }

        public async Task<StudentModel> UpdateStudentAsync(UpdateStudentModel studentModel)
        {
            try
            {
                var foundStudent = await _unitOfWork.StudentRepository.GetByIdAsync(studentModel.Id);

                if (foundStudent == null)
                {
                    return null;
                }

                foundStudent.Account.Email = studentModel.Email ?? foundStudent.Account.Email;
                foundStudent.Account.FirstName = studentModel.FirstName ?? foundStudent.Account.FirstName;
                foundStudent.Account.LastName = studentModel.LastName ?? foundStudent.Account.LastName;

                if (!string.IsNullOrEmpty(studentModel.Password))
                {
                    foundStudent.Account.Salt = _accountService.GenerateSalt();
                    foundStudent.Account.Password = _accountService.HashPassword(studentModel.Password, foundStudent.Account.Salt);
                }

                if (studentModel.StudentGroupIds != null)
                {
                    var currentStudentGroupsOfStudent = foundStudent.StudentsOfStudentGroups;
                    var newStudentsOfStudentGroup = new List<StudentOfStudentGroup>();

                    //for (int i = 0; i < studentGroupModel.StudentIds.Count; i++)
                    //    foundStudentGroup.StudentsOfStudentGroups.Add(new StudentOfStudentGroup { StudentId = foundStudents[i] });

                    foreach (var newStudentGroupId in studentModel.StudentGroupIds)
                    {
                        newStudentsOfStudentGroup.Add(new StudentOfStudentGroup
                        {
                            StudentGroupId = newStudentGroupId,
                            StudentId = foundStudent.Id
                        });
                    }

                    _unitOfWork.StudentGroupRepository.UpdateManyToMany(currentStudentGroupsOfStudent, newStudentsOfStudentGroup);
                }

                await _unitOfWork.CommitAsync();

                return _mapper.Map<StudentModel>(foundStudent);

            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<StudentModel> GetStudentByAccountIdAsync(long accountId)
        {
            var student = await _unitOfWork.StudentRepository.GetStudentByAccountIdAsync(accountId);

            return _mapper.Map<StudentModel>(student);
        }

        public async Task<long?> GetAccountId(long studentId)
        {
            var mentor = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);

            return mentor?.AccountId;
        }

        public async Task<StudentModel> GetStudentByIdAsync(long studentId)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);

            return _mapper.Map<StudentModel>(student);
        }

        public async Task<StudentModel> GetStudentByEmailAsync(string email)
        {
            var student = await _unitOfWork.StudentRepository.GetStudentByEmailAsync(email);

            return _mapper.Map<StudentModel>(student);
        }
    }
}