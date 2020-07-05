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
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    // How to set password?
                    var account = await _accountService.CreateAccountAsync(new Account
                    {
                        Email = mentorModel.Email,
                        FirstName = mentorModel.FirstName,
                        LastName = mentorModel.LastName,
                        Password = "temp",
                        Role = 2
                    }.ToAccountModel());

                    var mentor = new Mentor { AccountId = account.Id };
                    _unitOfWork.MentorRepository.Add(mentor);

                    await _unitOfWork.CommitAsync();

                    if (mentorModel.Courses_id != null)
                        // TODO: access via service, not uof
                        for (int i = 0; i < mentorModel.Courses_id.Length; i++)
                        {
                            _unitOfWork.MentorOfCourseRepository.Add(new MentorOfCourse
                            {
                                MentorId = mentor.Id,
                                CourseId = mentorModel.Courses_id[i]
                            });
                        }

                    await _unitOfWork.CommitAsync();

                    await transaction.CommitAsync();

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
                mentorModels.Add(mentor.ToMentorModel());
            }
            return mentorModels;
        }
    }
}
