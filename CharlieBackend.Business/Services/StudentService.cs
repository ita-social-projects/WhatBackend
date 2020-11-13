using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notification;

        public StudentService(IAccountService accountService, IUnitOfWork unitOfWork, 
                              IMapper mapper, INotificationService notification)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notification = notification;
        }

        public async Task<Result<StudentDto>> CreateStudentAsync(long accountId)
        {
            try
            {
                var account = await _accountService.GetAccountCredentialsByIdAsync(accountId);

                if (account == null)
                {
                    return Result<StudentDto>.Error(ErrorCode.NotFound,
                        "Account not found");
                }

                if (account.Role == UserRole.NotAssigned)
                {
                    account.Role = UserRole.Student;


                    var student = new Student
                    {
                        Account = account,
                        AccountId = accountId
                    };

                    _unitOfWork.StudentRepository.Add(student);

                    await _unitOfWork.CommitAsync();

                    await _notification.AccountApproved(account);

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

        public async Task<Result<StudentDto>> UpdateStudentAsync(long studentId, UpdateStudentDto studentModel)
        {
            try
            {
                var foundStudent = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);

                if (foundStudent == null)
                {
                        return Result<StudentDto>.Error(ErrorCode.NotFound, "Student not found");
                }

                var isEmailChangableTo = await _accountService
                    .IsEmailChangableToAsync((long)foundStudent.AccountId, studentModel.Email);

                if (!isEmailChangableTo)
                {
                        return Result<StudentDto>.Error(ErrorCode.ValidationError,
                        "Email is already taken!");
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