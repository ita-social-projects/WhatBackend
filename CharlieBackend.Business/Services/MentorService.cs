using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Extensions;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class MentorService : IMentorService
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notification;
        private readonly IBlobService _blobService;
        private readonly ICurrentUserService _currentUserService;

        public MentorService(IAccountService accountService, IUnitOfWork unitOfWork,
                              IMapper mapper, INotificationService notification,
                              IBlobService blobService,
                              ICurrentUserService currentUserService)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notification = notification;
            _blobService = blobService;
            _currentUserService = currentUserService;
        }

        public async Task<Result<MentorDto>> CreateMentorAsync(long accountId)
        {
            try
            {
                var account = await _accountService.GetAccountCredentialsByIdAsync(accountId);

                if (account == null)
                {
                    return Result<MentorDto>.GetError(ErrorCode.NotFound,
                        "Account not found");
                }

                if (account.Role == UserRole.NotAssigned)
                {
                    account.Role = UserRole.Mentor;


                    var mentor = new Mentor
                    {
                        Account = account,
                        AccountId = accountId
                    };

                    _unitOfWork.MentorRepository.Add(mentor);

                    await _unitOfWork.CommitAsync();

                    await _notification.AccountApproved(account);

                    return Result<MentorDto>.GetSuccess(_mapper.Map<MentorDto>(mentor));
                }
                else
                {
                    _unitOfWork.Rollback();

                    return Result<MentorDto>.GetError(ErrorCode.ValidationError,
                        "This account already assigned.");
                }
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<MentorDto>.GetError(ErrorCode.InternalServerError,
                     "Cannot create mentor.");
            }
        }

        public async Task<IList<MentorDetailsDto>> GetAllMentorsAsync()
        {
            var mentors = await GetMentorsWithAvatarIncluded(await _unitOfWork.MentorRepository.GetAllAsync());

            return mentors;
        }

        private async Task<IList<MentorDetailsDto>> GetMentorsWithAvatarIncluded(IList<Mentor> mentors)
        {
            var detailsDtos = await mentors
                .ToAsyncEnumerable()
                .Select(m => 
                {
                    var detailsDto = _mapper.Map<MentorDetailsDto>(m);

                    detailsDto.AvatarUrl = m.Account.Avatar != null ? _blobService.GetUrl(m.Account.Avatar) : null;

                    return detailsDto;
                })
                .Select(x => x)
                .ToListAsync();

            return detailsDtos;
        }

        public async Task<IList<MentorDetailsDto>> GetAllActiveMentorsAsync()
        {
            var mentors = await GetMentorsWithAvatarIncluded(await _unitOfWork.MentorRepository.GetAllActiveAsync());

            return mentors;
        }

        public async Task<Result<MentorDto>> UpdateMentorAsync(long mentorId, UpdateMentorDto mentorModel)
        {
            try
            {
                var foundMentor = await _unitOfWork.MentorRepository.GetByIdAsync(mentorId);

                if (foundMentor == null)
                {
                    return Result<MentorDto>.GetError(ErrorCode.NotFound,
                        "Mentor not found");
                }

                if (mentorModel.StudentGroupIds != null && mentorModel.StudentGroupIds.Any())
                {
                    var dublicatesGroup = mentorModel.StudentGroupIds.Dublicates();

                    if (dublicatesGroup.Any())
                    {
                        return Result<MentorDto>.GetError(ErrorCode.ValidationError, $"Such student group ids: {string.Join(" ", dublicatesGroup)} are not unique");
                    }

                }

                if (mentorModel.CourseIds != null && mentorModel.CourseIds.Any())
                {
                    var dublicatesCourse = mentorModel.CourseIds.Dublicates();

                    if (dublicatesCourse.Any())
                    {
                        return Result<MentorDto>.GetError(ErrorCode.ValidationError, $"Such course ids: {string.Join(" ", dublicatesCourse)} are not unique");
                    }

                }

                var isEmailChangableTo = await _accountService
                        .IsEmailChangableToAsync((long)foundMentor.AccountId, mentorModel.Email);

                if (!isEmailChangableTo)
                {
                    return Result<MentorDto>.GetError(ErrorCode.ValidationError,
                        "Email is already taken!");
                }

                foundMentor.Account.Email = mentorModel.Email ?? foundMentor.Account.Email;
                foundMentor.Account.FirstName = mentorModel.FirstName ?? foundMentor.Account.FirstName;
                foundMentor.Account.LastName = mentorModel.LastName ?? foundMentor.Account.LastName;

                if (mentorModel.CourseIds != null)
                {
                    var currentMentorCourses = foundMentor.MentorsOfCourses;
                    var newMentorCourses = new List<MentorOfCourse>();

                    foreach (var newCourseId in mentorModel.CourseIds)
                    {
                        newMentorCourses.Add(new MentorOfCourse
                        {
                            CourseId = newCourseId,
                            MentorId = foundMentor.Id
                        });
                    }

                    _unitOfWork.MentorRepository.UpdateMentorCourses(currentMentorCourses, newMentorCourses);
                }

                if (mentorModel.StudentGroupIds != null)
                {
                    var currentMentorGroups = foundMentor.MentorsOfStudentGroups;
                    var newMentorGroups = new List<MentorOfStudentGroup>();

                    foreach (var newGroupId in mentorModel.StudentGroupIds)
                    {
                        newMentorGroups.Add(new MentorOfStudentGroup
                        {
                            StudentGroupId = newGroupId,
                            MentorId = foundMentor.Id
                        });
                    }

                    _unitOfWork.MentorRepository.UpdateMentorGroups(currentMentorGroups, newMentorGroups);
                }

                await _unitOfWork.CommitAsync();

                return Result<MentorDto>.GetSuccess(_mapper.Map<MentorDto>(foundMentor));
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<MentorDto>.GetError(ErrorCode.InternalServerError,
                      "Cannot update mentor.");
            }
        }

        public async Task<Result<MentorDto>> GetMentorByAccountIdAsync(long accountId)
        {
            var mentor = await _unitOfWork.MentorRepository.GetMentorByAccountIdAsync(accountId);
            var mentorDto = _mapper.Map<MentorDto>(mentor);

            if (mentorDto == null)
            {
                return Result<MentorDto>.GetError(ErrorCode.NotFound, "Not Found");
            }

            return Result<MentorDto>.GetSuccess(mentorDto);
        }

        public async Task<Result<MentorDto>> GetMentorByIdAsync(long mentorId)
        {
            var mentor = await _unitOfWork.MentorRepository.GetByIdAsync(mentorId);
            if (mentor == null)
            {
                return Result<MentorDto>.GetError(ErrorCode.NotFound, "Mentor not found");
            }
            return Result<MentorDto>.GetSuccess(_mapper.Map<MentorDto>(mentor));
        }

        public async Task<long?> GetAccountIdAsync(long mentorId)
        {
            var mentor = await _unitOfWork.MentorRepository.GetByIdAsync(mentorId);

            return mentor?.AccountId;
        }


        public async Task<Result<bool>> DisableMentorAsync(long mentorId)
        {
            var accountId = await GetAccountIdAsync(mentorId);

            if (accountId == null)
            {
                return Result<bool>.GetError(ErrorCode.NotFound, "Unknown mentor id.");
            }

            var changedToDisabled = await _accountService.DisableAccountAsync(accountId.Value);

            if (!changedToDisabled)
            {
                return Result<bool>.GetError(ErrorCode.Conflict, "This account is already disabled.");
            }

            return Result<bool>.GetSuccess(changedToDisabled);

        }

        public async Task<Result<bool>> EnableMentorAsync(long mentorId)
        {
            var accountId = await GetAccountIdAsync(mentorId);

            if (accountId == null)
            {
                return Result<bool>.GetError(ErrorCode.NotFound, "Unknown mentor id.");
            }

            var changedToEnabled = await _accountService.EnableAccountAsync(accountId.Value);

            if (!changedToEnabled)
            {
                return Result<bool>.GetError(ErrorCode.Conflict, "This account is already enabled.");
            }

            return Result<bool>.GetSuccess(changedToEnabled);

        }

        public async Task<IList<MentorStudyGroupsDto>> GetMentorStudyGroupsByMentorIdAsync(long id)
        {
            var foundGroups = await _unitOfWork.StudentGroupRepository.GetMentorStudyGroups(id);

            if (foundGroups == null)
            {
                return new List<MentorStudyGroupsDto>();
            }

            return foundGroups;
        }

        public async Task<IList<MentorCoursesDto>> GetMentorCoursesByMentorIdAsync(long id)
        {
            var foundCourses = await _unitOfWork.CourseRepository.GetMentorCoursesAsync(id);

            if (foundCourses == null)
            {
                return new List<MentorCoursesDto>();
            }

            return foundCourses;
        }

        /// <summary>
        /// Return result of checking to role of mentor and it's identity numbers
        /// </summary>
        /// <typeparam name="T">Type of data</typeparam>
        /// <param name="id">Id of user entity</param>
        /// <param name="result">Result of checking to role and
        /// equality identity numbers of entities with type of data</param>
        /// <returns></returns>
        public Result<T> CheckRoleAndIdMentor<T>(long id)
        {
            Result<T> result = new Result<T>();

            if (_currentUserService.Role.Is(UserRole.Mentor)
                  && _currentUserService.EntityId != id)
            {
                result = Result<T>.GetError(ErrorCode.Unauthorized,
                        "Mentor can get only his information");
            }

            return result;
        }
    }
}
