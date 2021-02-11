using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.Extensions;
using System.Linq;

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
                    return Result<StudentDto>.GetError(ErrorCode.NotFound,
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

                    return Result<StudentDto>.GetSuccess(_mapper.Map<StudentDto>(student));
                }
                else
                {
                    _unitOfWork.Rollback();

                    return Result<StudentDto>.GetError(ErrorCode.ValidationError,
                        "This account already assigned.");
                }
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<StudentDto>.GetError(ErrorCode.InternalServerError,
                    "Cannot create student.");
            }
        }

        public async Task<Result<IList<StudentDto>>> GetAllStudentsAsync()
        {
            var students = _mapper.Map<IList<StudentDto>>(await _unitOfWork.StudentRepository.GetAllAsync());

            return Result<IList<StudentDto>>.GetSuccess(students);
        }

        public async Task<Result<IList<StudentDto>>> GetAllActiveStudentsAsync()
        {
            var students = _mapper.Map<IList<StudentDto>>(await _unitOfWork.StudentRepository.GetAllActiveAsync());

            return Result<IList<StudentDto>>.GetSuccess(students);
        }

        public async Task<Result<IList<StudentStudyGroupsDto>>> GetStudentStudyGroupsByStudentIdAsync(long id)
        {
            if (!await _unitOfWork.StudentRepository.IsEntityExistAsync(id))
            {
                return Result<IList<StudentStudyGroupsDto>>.GetError(ErrorCode.NotFound, "Student doesn`t exist");
            }

            var foundGroups = await _unitOfWork.StudentGroupRepository.GetStudentStudyGroups(id);

            if (!foundGroups.Any())
            {
                return Result<IList<StudentStudyGroupsDto>>.GetError(ErrorCode.NotFound, $"Study groups for student with id {id} not found");
            }

            return Result<IList<StudentStudyGroupsDto>>.GetSuccess(foundGroups);
        }

        public async Task<Result<StudentDto>> UpdateStudentAsync(long studentId, UpdateStudentDto studentModel)
        {
            try
            {
                var foundStudent = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);

                if (foundStudent == null)
                {
                    return Result<StudentDto>.GetError(ErrorCode.NotFound, "Student not found");
                }

                if (studentModel.StudentGroupIds != null && studentModel.StudentGroupIds.Any())
                {
                    var dublicates = studentModel.StudentGroupIds.Dublicates();

                    if (dublicates.Any())
                    {
                        return Result<StudentDto>.GetError(ErrorCode.ValidationError, $"Such student group ids: {string.Join(" ",dublicates)} are not unique");
                    }

                }

                var isEmailChangableTo = await _accountService
                    .IsEmailChangableToAsync((long)foundStudent.AccountId, studentModel.Email);

                if (!isEmailChangableTo)
                {
                    return Result<StudentDto>.GetError(ErrorCode.ValidationError,
                    "Email is already taken!");
                }

                foundStudent.Account.Email = studentModel.Email ?? foundStudent.Account.Email;
                foundStudent.Account.FirstName = studentModel.FirstName ?? foundStudent.Account.FirstName;
                foundStudent.Account.LastName = studentModel.LastName ?? foundStudent.Account.LastName;

                if (studentModel.StudentGroupIds?.Count > 0)
                {
                    _unitOfWork.StudentGroupRepository.UpdateManyToMany(foundStudent.StudentsOfStudentGroups,
                                                                        studentModel.StudentGroupIds.Select(x => new StudentOfStudentGroup
                                                                        {
                                                                            StudentGroupId = x,
                                                                            StudentId = studentId
                                                                        }));
                }

                await _unitOfWork.CommitAsync();

                return Result<StudentDto>.GetSuccess(_mapper.Map<StudentDto>(foundStudent));

            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<StudentDto>.GetError(ErrorCode.InternalServerError,
                      "Cannot update student.");
            }
        }

        public async Task<Result<StudentDto>> GetStudentByAccountIdAsync(long accountId)
        {
            var student = await _unitOfWork.StudentRepository.GetStudentByAccountIdAsync(accountId);
            var studentDto = _mapper.Map<StudentDto>(student);

            return Result<StudentDto>.GetSuccess(studentDto);
        }

        public async Task<long?> GetAccountId(long studentId)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);

            return student?.AccountId;
        }

        public async Task<Result<StudentDto>> GetStudentByIdAsync(long studentId)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);

            if (student == null)
            {
                return Result<StudentDto>.GetError(ErrorCode.NotFound, $"This id = {studentId} does not exist in database");
            }

            var studentDto = _mapper.Map<StudentDto>(student);

            return Result<StudentDto>.GetSuccess(studentDto);
        }

        public async Task<Result<StudentDto>> GetStudentByEmailAsync(string email)
        {
            var student = await _unitOfWork.StudentRepository.GetStudentByEmailAsync(email);
            var studentDto = _mapper.Map<StudentDto>(student);

            return Result<StudentDto>.GetSuccess(studentDto);
        }

        public async Task<Result<StudentDto>> DisableStudentAsync(long id)
        {
            var accountId = await GetAccountId(id);

            if (accountId == null)
            {
                return Result<StudentDto>.GetError(ErrorCode.NotFound, "Unknown student id.");
            }

            var student = await GetStudentByAccountIdAsync((long)accountId);
            var isActive = await _accountService.DisableAccountAsync((long)accountId);

            if (!isActive)
            {
                return Result<StudentDto>.GetError(ErrorCode.NotFound, "This account is already disabled.");
            }

            return Result<StudentDto>.GetSuccess(student.Data);
        }
    }
}