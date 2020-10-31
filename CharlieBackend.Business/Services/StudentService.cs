using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using AutoMapper;
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

        public StudentService(IAccountService accountService, IUnitOfWork unitOfWork, 
                              ICredentialsSenderService credentialsSender,
                              IMapper mapper)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _credentialSender = credentialsSender;
            _mapper = mapper;
        }

        /*public async Task<StudentModel> CreateStudentAsync(CreateStudentModel studentModel)
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

                    var credentialMessageSent = false;

                    if (await _credentialSender.SendCredentialsAsync(account.Email, generatedPassword))
                    {
                        await transaction.CommitAsync();

                        credentialMessageSent = true;

                        return student.ToStudentModel();
                    }
                    else
                    {
                        //Have to implement here sending error message or details to calling method/controller
                        //throw new Exception("Email has not been sent");

                        await transaction.CommitAsync();

                        credentialMessageSent = false;

                        return student.ToStudentModel();
                    }

                }
                catch
                {
                    //Have to implement here sending error message or details to calling method/controller
                    transaction.Rollback();

                    return null;
                }
            }
        }*/

        public async Task<IList<StudentDto>> GetAllStudentsAsync()
        {
            var students = _mapper.Map<List<StudentDto>>(await _unitOfWork.StudentRepository.GetAllAsync());

            return students;
        }

        /*public async Task<StudentModel> UpdateStudentAsync(UpdateStudentModel studentModel)
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

                return foundStudent.ToStudentModel();

            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }*/

        public async Task<StudentDto> GetStudentByAccountIdAsync(long accountId)
        {
            var student = await _unitOfWork.StudentRepository.GetStudentByAccountIdAsync(accountId);

            return _mapper.Map<StudentDto>(student);
        }

        public async Task<long?> GetAccountId(long studentId)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);

            return student?.AccountId;
        }

        public async Task<StudentDto> GetStudentByIdAsync(long studentId)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);

            return _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> GetStudentByEmailAsync(string email)
        {
            var student = await _unitOfWork.StudentRepository.GetStudentByEmailAsync(email);

            return _mapper.Map<StudentDto>(student);
        }
    }
}