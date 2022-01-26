using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Panel.Models.Course;
using CharlieBackend.Panel.Models.Homework;
using CharlieBackend.Panel.Models.Lesson;
using CharlieBackend.Panel.Models.Mentor;
using CharlieBackend.Panel.Models.StudentGroups;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeworksController : Controller
    {
        private readonly IHomeworkService _homeworkService;
        private readonly ICourseService _courseService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly ILessonService _lessonService;
        private readonly IMentorService _mentorService;

        public HomeworksController(IHomeworkService homeworkService,
            ICourseService courseService, IStudentGroupService studentGroupService, ILessonService lessonService, 
            IMentorService mentorService)
        {
            _homeworkService = homeworkService;
            _courseService = courseService;
            _studentGroupService = studentGroupService;
            _lessonService = lessonService;
            _mentorService = mentorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var homeworks = await _homeworkService.GetHomeworks();
            homeworks = homeworks.OrderBy(x => x.PublishingDate).ToList();

            return View(homeworks);
        }

        [HttpGet]
        public async Task<IActionResult> SelectCourses()
        {
            IEnumerable<CourseViewModel> courses = await _courseService.GetAllCoursesAsync();

            return View("Step1Courses", courses);
        }

        [HttpGet()]
        public async Task<IActionResult> SelectGroups(long courseId)
        {
            IEnumerable<StudentGroupViewModel> stGroups = await _studentGroupService.GetAllStudentGroupsAsync();
            IEnumerable<StudentGroupViewModel> groups = stGroups.Where(x => x.Course.Id == courseId).ToList();
            return View("Step2Groups", groups);
        }

        [HttpGet("{stGroupId}/{themeN?}/{mentorId?}")]
        public async Task<IActionResult> SelectLessonId(long stGroupId, string themeN, long? mentorId)
        {
            List<MentorEditViewModel> mentors = new List<MentorEditViewModel>();
            
            IEnumerable<LessonViewModel> allLessons = await _lessonService.GetLessonsByDateAsync();
            if (themeN == null && mentorId == null)
            {
                IEnumerable<string> lessonThemes = allLessons.Where(x => x.StudentGroupId == stGroupId)
                    .Select(x => x.ThemeName).Distinct().ToList();
                return View("Step3ThemeNames", lessonThemes);
            }
            IEnumerable<long> mentorsId = allLessons.Where(x => x.StudentGroupId == stGroupId)
                .Where(z => z.ThemeName == themeN).Select(x => x.MentorId).Distinct().ToList();
            if (mentorId == null)
            {
                foreach (var id in mentorsId)
                {
                    MentorEditViewModel mentor = await _mentorService.GetMentorByIdAsync(id);
                    mentors.Add(mentor);
                }
                return View("Step4Mentors", mentors);
            }
            IEnumerable<LessonViewModel> lessons = allLessons.Where(x => x.StudentGroupId == stGroupId)
                .Where(z => z.ThemeName == themeN).Where(d => d.MentorId == mentorId).ToList();
            return View("Step5LessonsDate", lessons);
        }

        [HttpGet("{lessonId}")]
        public async Task<IActionResult> CreateHomework(long lessonId)
        {

            HomeworkDto homework = new HomeworkDto
            {
                LessonId = lessonId
            };

            return View("Create", homework);
        }

        [HttpPost]
        public async Task<IActionResult> PostHomework(HomeworkDto homework)
        {
            await _homeworkService.AddHomeworkEndpoint(homework);

            return RedirectToAction("Index", "Homeworks"); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> PrepareHomeworkForUpdate(long id)
        {
            HomeworkViewModel homework = await _homeworkService.GetHomeworkById(id);

            return View("Edit", homework);
        }

        [HttpPost()]
        public async Task<IActionResult> Edit(long id, HomeworkDto homework)
        {
            await _homeworkService.UpdateHomeworkEndpoint(id, homework);

            return RedirectToAction("Index", "Homeworks");
        }
    }
}
