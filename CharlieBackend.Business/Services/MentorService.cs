using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                // How to set password?
                var account = await _accountService.CreateAccountAsync(new Account
                {
                    Email = mentorModel.login,
                    FirstName = mentorModel.FirstName,
                    LastName = mentorModel.LastName,
                    Password = "temp",
                    Role = 2
                }.ToAccountModel());

                var mentor = new Mentor { AccountId = account.Id };
                _unitOfWork.MentorRepository.Add(mentor);

                await _unitOfWork.CommitAsync();

                if (mentorModel.courses_id != null)
                    // TODO: access via service, not uof
                    for (int i = 0; i < mentorModel.courses_id.Length; i++)
                    {
                        _unitOfWork.MentorOfCourseRepository.Add(new MentorOfCourse
                        {
                            MentorId = mentor.Id,
                            CourseId = mentorModel.courses_id[i]
                        });
                    }

                await _unitOfWork.CommitAsync();

                return mentorModel;
            }
            catch { _unitOfWork.Rollback(); return null; }
        }

        //public async Task<List<MentorModel>> GetAllMentorsAsync()
        //{
        //    var mentors = _unitOfWork.MentorRepository.GetAllAsync();

        //}
    }
}
