using EasyNetQ;
using AutoMapper;
using CharlieBackend.Core;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.IntegrationEvents.Events;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using EasyNetQ;
using EasyNetQ.Topology;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;

namespace CharlieBackend.Business.Services
{
    public class MentorService : IMentorService
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICredentialsSenderService _credentialsSender;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public MentorService(IAccountService accountService, IUnitOfWork unitOfWork, ICredentialsSenderService credentialsSender,
                             IMapper mapper, IBus bus)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _credentialsSender = credentialsSender;
            _mapper = mapper;
            _bus = bus;
        }

        public async Task<Result<MentorDto>> CreateMentorAsync(long accountId)
        {
            try
            {
                var account = await _accountService.GetAccountCredentialsByIdAsync(accountId);
                
                if (account.Role == Roles.NotAssigned)
                {
                    account.Role = Roles.Mentor;


                    var mentor = new Mentor
                    {
                        Account = account,
                        AccountId = accountId
                    };

                    _unitOfWork.MentorRepository.Add(mentor);

                    await _unitOfWork.CommitAsync();

                    await _bus.PubSub.PublishAsync(new AccountApprovedEvent(account.Email,
                                        account.FirstName, account.LastName, account.Role), 
                                        "EmailRenderService");

                    return Result<MentorDto>.Success(_mapper.Map<MentorDto>(mentor));
                }
                else
                {
                    _unitOfWork.Rollback();

                    return Result<MentorDto>.Error(ErrorCode.ValidationError,
                        "This account already assigned.");
                }
            }
            catch
            {
                 _unitOfWork.Rollback();

                 return Result<MentorDto>.Error(ErrorCode.InternalServerError,
                      "Cannot create mentor.");
            }

        }

        public async Task<IList<MentorDto>> GetAllMentorsAsync()
        {

            var mentors = _mapper.Map<List<MentorDto>>(await _unitOfWork.MentorRepository.GetAllAsync());

            return mentors;
        }

        public async Task<Result<MentorDto>> UpdateMentorAsync(long mentorId, UpdateMentorDto mentorModel)
        {
            try
            {
                var foundMentor = await _unitOfWork.MentorRepository.GetByIdAsync(mentorId);

                if (foundMentor == null)
                {
                    return Result<MentorDto>.Error(ErrorCode.ValidationError,
                        "Mentor not found");
                }

                var isEmailChangableTo = await _accountService
                        .IsEmailChangableToAsync((long)foundMentor.AccountId, mentorModel.Email);

                if (!isEmailChangableTo)
                {
                    return Result<MentorDto>.Error(ErrorCode.ValidationError,
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

                return Result<MentorDto>.Success(_mapper.Map<MentorDto>(foundMentor));
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<MentorDto>.Error(ErrorCode.InternalServerError,
                      "Cannot update mentor.");
            }
        }

        public async Task<MentorDto> GetMentorByAccountIdAsync(long accountId)
        {
            var mentor = await _unitOfWork.MentorRepository.GetMentorByAccountIdAsync(accountId);

            return _mapper.Map<MentorDto>(mentor);
        }
        public async Task<MentorDto> GetMentorByIdAsync(long mentorId)
        {
            var mentor = await _unitOfWork.MentorRepository.GetMentorByIdAsync(mentorId);

            return _mapper.Map<MentorDto>(mentor);
        }

        public async Task<long?> GetAccountId(long mentorId)
        {
            var mentor = await _unitOfWork.MentorRepository.GetByIdAsync(mentorId);

            return mentor?.AccountId;
        }
    }
}
