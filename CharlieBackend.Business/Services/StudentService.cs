using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Extensions;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IAccountService _accountService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notification;
        private readonly IBlobService _blobService;

        public StudentService(IAccountService accountService, IUnitOfWork unitOfWork,
                              IMapper mapper, INotificationService notification, IBlobService blobService, 
                              ICurrentUserService currentUserService)
        {
            _accountService = accountService;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notification = notification;
            _blobService = blobService;
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

        public async Task<Result<IList<StudentDetailsDto>>> GetAllStudentsAsync()
        {
            var students = await GetStudentsWithAvatarIncluded(await _unitOfWork.StudentRepository.GetAllAsync());

            return Result<IList<StudentDetailsDto>>.GetSuccess(students);
        }

        public async Task<Result<IList<StudentDetailsDto>>> GetAllActiveStudentsAsync()
        {
            var students = await GetStudentsWithAvatarIncluded(await _unitOfWork.StudentRepository.GetAllActiveAsync());

            return Result<IList<StudentDetailsDto>>.GetSuccess(students);
        }

        private async Task<IList<StudentDetailsDto>> GetStudentsWithAvatarIncluded(IList<Student> students)
        {

            var detailsDtos = await students
                .ToAsyncEnumerable()
                .Select(m =>
                {
                    var detailsDto = _mapper.Map<StudentDetailsDto>(m);

                    detailsDto.AvatarUrl = m.Account.Avatar != null ? _blobService.GetUrl(m.Account.Avatar) : null;

                    return detailsDto;
                })
                .Select(x => x)
                .ToListAsync();

            return detailsDtos;
         }

        public async Task<Result<IList<StudentStudyGroupsDto>>> GetStudentStudyGroupsByStudentIdAsync(long id)
        {
            if (_currentUserService.Role == UserRole.Student)
            {
                if (_currentUserService.EntityId != id)
                {
                    return Result<IList<StudentStudyGroupsDto>>.GetError(ErrorCode.Unauthorized, "Student can see only own student groups");
                }
            }

            if (!await _unitOfWork.StudentRepository.IsEntityExistAsync(id))
            {
                return Result<IList<StudentStudyGroupsDto>>.GetError(ErrorCode.NotFound, "Student doesn`t exist");
            }

            var foundGroups = await _unitOfWork.StudentGroupRepository.GetStudentStudyGroups(id);

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

            if (studentDto == null)
            {
                return Result<StudentDto>.GetError(ErrorCode.NotFound, "Not Found");
            }

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
            var student = await _unitOfWork.StudentRepository
                    .GetStudentByEmailAsync(email);

            if (student != null)
            {
                var studentDto = _mapper.Map<StudentDto>(student);

                return Result<StudentDto>.GetSuccess(studentDto);
            }

            return Result<StudentDto>.GetError(ErrorCode.NotFound,
                    "Student wasn't found by email");
        }

        public async Task<Result<bool>> DisableStudentAsync(long id)
        {
            var accountId = await GetAccountId(id);

            if (accountId == null)
            {
                return Result<bool>.GetError(ErrorCode.NotFound, "Unknown student id.");
            }

            var changedToDisabled = await _accountService.DisableAccountAsync((long)accountId);

            if (!changedToDisabled)
            {
                return Result<bool>.GetError(ErrorCode.Conflict, "This account is already disabled.");
            }

            return Result<bool>.GetSuccess(changedToDisabled);
        }

        public async Task<Result<bool>> EnableStudentAsync(long id)
        {
            var accountId = await GetAccountId(id);

            if (accountId == null)
            {
                return Result<bool>.GetError(ErrorCode.NotFound, "Unknown student id.");
            }

            var changedToEnabled = await _accountService.EnableAccountAsync((long)accountId);

            if (!changedToEnabled)
            {
                return Result<bool>.GetError(ErrorCode.Conflict, "This account is already enabled.");
            }

            return Result<bool>.GetSuccess(changedToEnabled);
        }
    }
}
