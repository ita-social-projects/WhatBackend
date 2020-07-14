using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using CharlieBackend.Core.Models.Mentor;
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

        public MentorService(IAccountService accountService, IUnitOfWork unitOfWork, ICredentialsSenderService credentialsSender)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _credentialsSender = credentialsSender;
        }

        public async Task<MentorModel> CreateMentorAsync(CreateMentorModel mentorModel)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var generatedPassword = _accountService.GenerateSalt();
                    var account = new Account
                    {
                        Email = mentorModel.Email,
                        FirstName = mentorModel.FirstName,
                        LastName = mentorModel.LastName,
                        Role = 2
                    };
                    account.Salt = _accountService.GenerateSalt();
                    account.Password = _accountService.HashPassword(generatedPassword, account.Salt);

                    var mentor = new Mentor { Account = account };
                    _unitOfWork.MentorRepository.Add(mentor);

                    if (mentorModel.CourseIds.Count != 0)
                    {
                        var courses = await _unitOfWork.CourseRepository.GetCoursesByIdsAsync(mentorModel.CourseIds);
                        mentor.MentorsOfCourses = new List<MentorOfCourse>();

                        for (int i = 0; i < courses.Count; i++)
                            mentor.MentorsOfCourses.Add(new MentorOfCourse { Mentor = mentor, Course = courses[i] });
                    }

                    await _unitOfWork.CommitAsync();
                    await _credentialsSender.SendCredentialsAsync(account.Email, generatedPassword);

                    transaction.Commit();
                    return mentor.ToMentorModel();

                }
                catch { transaction.Rollback(); return null; }
            }

        }

        public async Task<List<MentorModel>> GetAllMentorsAsync()
        {
            var mentors = await _unitOfWork.MentorRepository.GetAllAsync();

            var mentorModels = new List<MentorModel>();

            foreach (var mentor in mentors)
            {
                var mentorModel = mentor.ToMentorModel();
                mentorModels.Add(mentorModel);
            }

            return mentorModels;
        }

        public async Task<MentorModel> UpdateMentorAsync(UpdateMentorModel mentorModel)
        {
            try
            {
                var foundMentor = await _unitOfWork.MentorRepository.GetByIdAsync(mentorModel.Id);
                if (foundMentor == null) return null;

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
                        newMentorCourses.Add(new MentorOfCourse { CourseId = newCourseId, MentorId = foundMentor.Id });

                    _unitOfWork.MentorRepository.UpdateMentorCourses(currentMentorCourses, newMentorCourses);
                }

                if (mentorModel.StudentGroupIds != null)
                {
                    var currentMentorGroups = foundMentor.MentorsOfStudentGroups;
                    var newMentorGroups = new List<MentorOfStudentGroup>();

                    foreach (var newGroupId in mentorModel.StudentGroupIds)
                        newMentorGroups.Add(new MentorOfStudentGroup { StudentGroupId = newGroupId, MentorId = foundMentor.Id });

                    _unitOfWork.MentorRepository.UpdateMentorGroups(currentMentorGroups, newMentorGroups);
                }

                await _unitOfWork.CommitAsync();
                return foundMentor.ToMentorModel();

            }
            catch { _unitOfWork.Rollback(); return null; }
        }

        public async Task<MentorModel> GetMentorByAccountIdAsync(long accountId)
        {
            var mentor = await _unitOfWork.MentorRepository.GetMentorByAccountIdAsync(accountId);
            return mentor?.ToMentorModel();
        }

        public async Task<long?> GetAccountId(long mentorId)
        {
            var mentor = await _unitOfWork.MentorRepository.GetByIdAsync(mentorId);
            return mentor?.AccountId;
        }
    }
}
