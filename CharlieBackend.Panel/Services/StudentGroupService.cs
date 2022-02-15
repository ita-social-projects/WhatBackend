﻿using AutoMapper;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using CharlieBackend.Panel.Models.Mentor;
using CharlieBackend.Panel.Models.StudentGroups;
using CharlieBackend.Panel.Models.Students;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    public class StudentGroupService : IStudentGroupService
    {
        private readonly IApiUtil _apiUtil;
        private readonly IMapper _mapper;
        private readonly IMentorService _mentorService;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly ICurrentUserService _currentUserService;
        private readonly StudentGroupsApiEndpoints _studentGroupsApiEndpoints;

        public StudentGroupService(IApiUtil apiUtil,
                                   IMapper mapper,
                                   IMentorService mentorService,
                                   IStudentService studentService,
                                   ICourseService courseService,
                                   IOptions<ApplicationSettings> options,
                                   ICurrentUserService currentUserService)
        {
            _apiUtil = apiUtil;
            _mapper = mapper;
            _mentorService = mentorService;
            _studentService = studentService;
            _courseService = courseService;
            _currentUserService = currentUserService;
            _studentGroupsApiEndpoints = options.Value.Urls.ApiEndpoints.StudentGroups;
        }

        public async Task<IList<StudentGroupViewModel>> GetAllStudentGroupsAsync(bool isAllGroups = true)
        {
            var getAllStudentGroupsEndpoint = _studentGroupsApiEndpoints.GetAllStudentGroupsEndpoint;

            var studentGroupsResponse = await _apiUtil.GetAsync<IList<StudentGroupDto>>(getAllStudentGroupsEndpoint);
            var studentGroups = _mapper.Map<IList<StudentGroupViewModel>>(studentGroupsResponse);

            var students = await _studentService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();
            IList<MentorViewModel> mentors = null;

            if (_currentUserService.Role == UserRole.Admin || _currentUserService.Role == UserRole.Secretary)
            {
                mentors = await _mentorService.GetAllMentorsAsync();
            }
            else if (_currentUserService.Role == UserRole.Mentor)
            {
                mentors = await _mentorService.GetAllActiveMentorsAsync();
            }

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

            if (_currentUserService.Role == UserRole.Mentor && !isAllGroups)
            {
                return studentGroups.Where(g => g.Mentors.Any(m => m.Id == _currentUserService.EntityId)).ToList();
            }

            return studentGroups;
        }

        public async Task<StudentGroupEditViewModel> PrepareStudentGroupAddAsync()
        {
            var studentsTask = _studentService.GetAllStudentsAsync();
            var mentorsTask = _mentorService.GetAllActiveMentorsAsync();
            var coursesTask = _courseService.GetAllCoursesAsync();

            var studentGroup = new StudentGroupEditViewModel
            {
                AllCourses = await coursesTask,
                AllStudents = await studentsTask,
                AllMentors = await mentorsTask
            };

            return studentGroup;
        }

        public async Task<StudentGroupEditViewModel> PrepareStudentGroupUpdateAsync(long id)
        {
            var getStudentGroupEndpoint = string
                .Format(_studentGroupsApiEndpoints.GetStudentGroupEndpoint, id);

            var studentGroupsTask = _apiUtil.GetAsync<StudentGroupDto>(getStudentGroupEndpoint);
            var studentsTask = _studentService.GetAllStudentsAsync();
            var mentorsTask = _mentorService.GetAllActiveMentorsAsync();

            var coursesTask = _courseService.GetAllCoursesAsync();

            var studentGroup = _mapper.Map<StudentGroupEditViewModel>(await studentGroupsTask);
            studentGroup.AllCourses = await coursesTask;
            studentGroup.AllStudents = await studentsTask;
            studentGroup.AllMentors = await mentorsTask;

            return studentGroup;
        }

        public async Task<StudentGroupDto> UpdateStudentGroupAsync(long id, StudentGroupDto updateDto)
        {
            var updateStudentGroupEndpoint = string
                .Format(_studentGroupsApiEndpoints.UpdateStudentGroupEndpoint, id);

            var updateStudentGroupTask = _apiUtil.PutAsync(updateStudentGroupEndpoint
                , _mapper.Map<UpdateStudentGroupDto>(updateDto));

            await updateStudentGroupTask;

            return updateDto;
        }

        public async Task<CreateStudentGroupDto> AddStudentGroupAsync(long id, CreateStudentGroupDto addDto)
        {
            var addStudentGroupEndpoint = _studentGroupsApiEndpoints.AddStudentGroupEndpoint;

            var createStudentGroup = await _apiUtil.CreateAsync(addStudentGroupEndpoint, addDto);

            return createStudentGroup;
        }

    }
}
