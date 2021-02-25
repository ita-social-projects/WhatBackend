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

        private readonly IMentorService _mentorService;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public StudentGroupService(IApiUtil apiUtil,
                                   IMapper mapper,

                                   IMentorService mentorService,
                                   IStudentService studentService,
                                   ICourseService courseService)
        {
            _apiUtil = apiUtil;
            _mapper = mapper;

            _mentorService = mentorService;
            _studentService = studentService;
            _courseService = courseService;
        }

        public async Task<IList<StudentGroupViewModel>> GetAllStudentGroupsAsync()
        {
            var studentGroupsResponse = await _apiUtil.GetAsync<IList<StudentGroupDto>>($"api/student_groups");
            var studentGroups = _mapper.Map<IList<StudentGroupViewModel>>(studentGroupsResponse);
            
            var students = await _studentService.GetAllStudentsAsync(); ;
            var mentors = await _mentorService.GetAllMentorsAsync();
            var courses = await _courseService.GetAllCoursesAsync();

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
            var studentGroupsTask = _apiUtil.GetAsync<StudentGroupDto>($"api/student_groups/{id}");
            var studentsTask = _studentService.GetAllStudentsAsync();
            var mentorsTask = _mentorService.GetAllMentorsAsync();
            var coursesTask = _courseService.GetAllCoursesAsync();

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
            var coursesTask = _courseService.GetAllCoursesAsync();

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
            var updateStudentGroupTask = _apiUtil.PutAsync($"api/student_groups/{id}", _mapper.Map<UpdateStudentGroupDto>(updateDto));


            await updateStudentGroupTask;

            return updateDto;
        }

        public async Task<CreateStudentGroupDto> AddStudentGroupAsync(long id, CreateStudentGroupDto addDto)
        {
            var createStudentGroup = await _apiUtil.CreateAsync($"api/student_groups", addDto);

            return createStudentGroup;
        }

    }
}
