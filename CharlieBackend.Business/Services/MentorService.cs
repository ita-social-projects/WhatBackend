using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO;
using CharlieBackend.Core.DTO.Mentor;
using AutoMapper;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class MentorService : IMentorService
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICredentialsSenderService _credentialsSender;
        private readonly IMapper _mapper;

        public MentorService(IAccountService accountService, IUnitOfWork unitOfWork, ICredentialsSenderService credentialsSender,
                             IMapper mapper)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _credentialsSender = credentialsSender;
            _mapper = mapper;
        }

        public async Task<MentorDto> CreateMentorAsync(long accountId)
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

                    return _mapper.Map<MentorDto>(mentor);
                }
                else
				{
                    _unitOfWork.Rollback();

                    return null;
                }

                }
                catch
                {
                    _unitOfWork.Rollback();

                    return null;
                }

        }

        public async Task<IList<MentorDto>> GetAllMentorsAsync()
        {
            var mentors = _mapper.Map<List<MentorDto>>(await _unitOfWork.MentorRepository.GetAllAsync());

            return mentors;
        }

        /*public async Task<MentorModel> UpdateMentorAsync(UpdateMentorModel mentorModel)
        {
            try
            {
                var foundMentor = await _unitOfWork.MentorRepository.GetByIdAsync(mentorModel.Id);

                if (foundMentor == null)
                {
                    return null;
                }

                foundMentor.Account.Email = mentorModel.Email ?? foundMentor.Account.Email;
                foundMentor.Account.FirstName = mentorModel.FirstName ?? foundMentor.Account.FirstName;
                foundMentor.Account.LastName = mentorModel.LastName ?? foundMentor.Account.LastName;

                if (!string.IsNullOrEmpty(mentorModel.Password))
                {
                    foundMentor.Account.Salt = _accountService.GenerateSalt();
                    foundMentor.Account.Password = _accountService.HashPassword(mentorModel.Password, foundMentor.Account.Salt);
                }

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

                return foundMentor.ToMentorModel();

            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }*/

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
