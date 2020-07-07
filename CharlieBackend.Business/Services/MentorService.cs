using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class MentorService : IMentorService
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;

        public MentorService(IAccountService accountService, IUnitOfWork unitOfWork)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
        }

        public async Task<MentorModel> CreateMentorAsync(MentorModel mentorModel)
        {
            try
            {
                var account = new Account
                {
                    Email = mentorModel.Email,
                    FirstName = mentorModel.FirstName,
                    LastName = mentorModel.LastName,
                    Role = 2
                };
                account.Salt = _accountService.GenerateSalt();
                account.Password = _accountService.HashPassword("temp", account.Salt);

                var mentor = new Mentor { Account = account };
                _unitOfWork.MentorRepository.Add(mentor);

                if (mentorModel.Courses_id.Count != 0)
                {
                    var courses = await _unitOfWork.CourseRepository.GetCoursesByIdsAsync(mentorModel.Courses_id);
                    mentor.MentorsOfCourses = new List<MentorOfCourse>();

                    for (int i = 0; i < courses.Count; i++)
                        mentor.MentorsOfCourses.Add(new MentorOfCourse { Mentor = mentor, Course = courses[i] });
                }

                await _unitOfWork.CommitAsync();

                return mentor.ToMentorModel();
            }
            catch { _unitOfWork.Rollback(); return null; }

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

        public async Task<MentorModel> UpdateMentorAsync(MentorModel mentorModel)
        {
            try
            {
                var foundMentor = await _unitOfWork.MentorRepository.GetByIdAsync(mentorModel.Id);

                foundMentor.Account.Email = mentorModel.Email;
                foundMentor.Account.FirstName = mentorModel.FirstName;
                foundMentor.Account.LastName = mentorModel.LastName;
                foundMentor.Account.Salt = _accountService.GenerateSalt();
                foundMentor.Account.Password = _accountService.HashPassword(mentorModel.Password, foundMentor.Account.Salt);

                var currentMentorCourses = foundMentor.MentorsOfCourses;
                var newMentorCourses = new List<MentorOfCourse>();

                foreach (var newCourseId in mentorModel.Courses_id)
                    newMentorCourses.Add(new MentorOfCourse { CourseId = newCourseId, MentorId = foundMentor.Id });

                _unitOfWork.MentorRepository.UpdateManyToMany(currentMentorCourses, newMentorCourses);

                await _unitOfWork.CommitAsync();
                return foundMentor.ToMentorModel();

            }
            catch { _unitOfWork.Rollback(); return null; }
        }
    }
}
