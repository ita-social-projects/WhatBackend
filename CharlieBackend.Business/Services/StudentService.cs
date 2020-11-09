using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using CharlieBackend.Core.Models.ResultModel;
using EasyNetQ;
using CharlieBackend.Core.IntegrationEvents.Events;

namespace CharlieBackend.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICredentialsSenderService _credentialSender;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public StudentService(IAccountService accountService, IUnitOfWork unitOfWork, 
                              ICredentialsSenderService credentialsSender,
                              IMapper mapper, IBus bus)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _credentialSender = credentialsSender;
            _mapper = mapper;
            _bus = bus;
        }

        public async Task<Result<StudentDto>> CreateStudentAsync(long accountId)
        {
            try
            {
                var account = await _accountService.GetAccountCredentialsByIdAsync(accountId);

                if (account.Role == Roles.NotAssigned)
                {
                    account.Role = Roles.Student;


                    var student = new Student
                    {
                        Account = account,
                        AccountId = accountId
                    };

                    _unitOfWork.StudentRepository.Add(student);

                    await _unitOfWork.CommitAsync();

                    _bus.PubSub.Publish(new AccountApprovedEvent(account.Email,
                                        account.FirstName, account.LastName, account.Role));

                    return Result<StudentDto>.Success(_mapper.Map<StudentDto>(student));
                }
                else
                {
                    _unitOfWork.Rollback();

                    return Result<StudentDto>.Error(ErrorCode.ValidationError,
                        "This account already assigned.");
                }
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<StudentDto>.Error(ErrorCode.InternalServerError,
                    "Cannot create student.");
            }
        }

        public async Task<IList<StudentDto>> GetAllStudentsAsync()
        {
            var students = _mapper.Map<List<StudentDto>>(await _unitOfWork.StudentRepository.GetAllAsync());

            return students;
        }

        public async Task<Result<StudentDto>> UpdateStudentAsync(long accountId, UpdateStudentDto studentModel)
        {
            try
            {
                var isEmailChangableTo = await _accountService
                    .IsEmailChangableToAsync(studentModel.Email);

                if (!isEmailChangableTo)
                {
                    return Result<StudentDto>.Error(ErrorCode.ValidationError,
                        "Email is already taken!");
                }

                var foundStudent = await _unitOfWork.StudentRepository.GetByIdAsync(accountId);

                if (foundStudent == null)
                {
                    return Result<StudentDto>.Error(ErrorCode.ValidationError,
                        "Student not found");
                }

                foundStudent.Account.Email = studentModel.Email ?? foundStudent.Account.Email;
                foundStudent.Account.FirstName = studentModel.FirstName ?? foundStudent.Account.FirstName;
                foundStudent.Account.LastName = studentModel.LastName ?? foundStudent.Account.LastName;

                if (studentModel.StudentGroupIds != null)
                {
                    var currentStudentGroupsOfStudent = foundStudent.StudentsOfStudentGroups;
                    var newStudentsOfStudentGroup = new List<StudentOfStudentGroup>();

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

                return Result<StudentDto>.Success(_mapper.Map<StudentDto>(foundStudent));

            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<StudentDto>.Error(ErrorCode.InternalServerError,
                      "Cannot update student.");
            }
        }

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