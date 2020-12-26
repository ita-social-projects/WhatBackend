using AutoMapper;
using CharlieBackend.AdminPanel.Models.Mentor;
using CharlieBackend.AdminPanel.Models.StudentGroups;
using CharlieBackend.AdminPanel.Models.Students;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.StudentGroups;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    public class StudentGroupService : IStudentGroupService
    {
        private readonly IApiUtil _apiUtil;
        private readonly IMapper _mapper;
        private readonly IOptions<ApplicationSettings> _config;
        private readonly IDataProtector _protector;

        private readonly IMentorService _mentorService;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly string _accessToken;

        public StudentGroupService(IApiUtil apiUtil,
                                   IMapper mapper,
                                   IOptions<ApplicationSettings> config,
                                   IHttpContextAccessor httpContextAccessor,
                                   IDataProtectionProvider provider,

                                   IMentorService mentorService,
                                   IStudentService studentService,
                                   ICourseService courseService)
        {
            _apiUtil = apiUtil;
            _config = config;
            _mapper = mapper;
            _protector = provider.CreateProtector(_config.Value.Cookies.SecureKey);

            _accessToken = _protector.Unprotect(httpContextAccessor.HttpContext.Request.Cookies["accessToken"]);

            _mentorService = mentorService;
            _studentService = studentService;
            _courseService = courseService;
        }

        public async Task<IList<StudentGroupViewModel>> GetAllStudentGroupsAsync()
        {
            var studentGroupsTask = _apiUtil.GetAsync<IList<StudentGroupDto>>($"{_config.Value.Urls.Api.Https}/api/student_groups", _accessToken);
            var studentsTask = _studentService.GetAllStudentsAsync();
            var mentorsTask = _mentorService.GetAllMentorsAsync();
            var coursesTask = _courseService.GetAllCoursesAsync(_accessToken);

            var studentGroups = _mapper.Map<IList<StudentGroupViewModel>>(await studentGroupsTask);
            var students = await studentsTask;
            var mentors = await mentorsTask;
            var courses = await coursesTask;

            foreach (var item in studentGroups)
            {
                item.Students = item.Students.Select(student =>
                {
                    var foundStudent = students.FirstOrDefault(x => x.Id == student.Id);

                    if (foundStudent != null)
                    {
                        return _mapper.Map<StudentViewModel>(foundStudent);
                    }
                    else
                    {
                        return student;
                    }
                }).ToList();

                item.Mentors = item.Mentors.Select(mentor =>
                {
                    var foundMentor = mentors.FirstOrDefault(x => x.Id == mentor.Id);

                    if (foundMentor != null)
                    {
                        return _mapper.Map<MentorViewModel>(foundMentor);
                    }
                    else
                    {
                        return mentor;
                    }
                }).ToList();

                item.Course.Name = courses.Where(x => x.Id == item.Course.Id).FirstOrDefault()?.Name;
            }

            return studentGroups;
        }

        public async Task<StudentGroupEditViewModel> PrepareStudentGroupUpdateAsync(long id)
        {
            var studentGroupsTask = _apiUtil.GetAsync<StudentGroupDto>($"{_config.Value.Urls.Api.Https}/api/student_groups/{id}", _accessToken);
            var studentsTask = _studentService.GetAllStudentsAsync();
            var mentorsTask = _mentorService.GetAllMentorsAsync();
            var coursesTask = _courseService.GetAllCoursesAsync(_accessToken);

            var studentGroup = _mapper.Map<StudentGroupEditViewModel>(await studentGroupsTask);
            studentGroup.AllCourses = await coursesTask;
            studentGroup.AllStudents = await studentsTask;
            studentGroup.AllMentors = await mentorsTask;

            return studentGroup;
        }

        public async Task<StudentGroupEditViewModel> PrepareStudentGroupAddAsync()
        {
            var studentsTask = _studentService.GetAllStudentsAsync();
            var mentorsTask = _mentorService.GetAllMentorsAsync();
            var coursesTask = _courseService.GetAllCoursesAsync(_accessToken);

            var studentGroup = new StudentGroupEditViewModel
            {
                AllCourses = await coursesTask,
                AllStudents = await studentsTask,
                AllMentors = await mentorsTask
            };

            return studentGroup;
        }

        public async Task<StudentGroupDto> UpdateStudentGroupAsync(long id, StudentGroupDto updateDto)
        {
            var updateStudentGroupTask = _apiUtil.PutAsync($"{_config.Value.Urls.Api.Https}/api/student_groups/{id}", _mapper.Map<UpdateStudentGroupDto>(updateDto), _accessToken);


            await updateStudentGroupTask;

            return updateDto;
        }

        public async Task<CreateStudentGroupDto> AddStudentGroupAsync(long id, CreateStudentGroupDto addDto)
        {
            var createStudentGroup = await _apiUtil.CreateAsync($"{_config.Value.Urls.Api.Https}/api/student_groups", addDto, _accessToken);

            return createStudentGroup;
        }

    }
}
