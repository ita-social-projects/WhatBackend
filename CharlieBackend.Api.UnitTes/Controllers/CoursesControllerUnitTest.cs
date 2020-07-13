using CharlieBackend.Api.Controllers;
using System.Collections.Generic;
using CharlieBackend.Business.Services.Interfaces;
using Moq;
using CharlieBackend.Core.Models.Course;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Microsoft.VisualBasic;
using System.Net.Http.Headers;

namespace CharlieBackend.Api.Controllers.Tests
{

	public class CoursesControllerUnitTest
	{
		//[Fact]
		//public void CoursesControllerTest()
		//{

		//}

		[Fact]
		public void PostCourseTest()
		{

			var mock = new Mock<ICourseService>();
			mock.Setup(repo => repo.GetAllCoursesAsync()).Returns(courses);
			var newCourse = new CreateCourseModel { Id = 232, Name = "Test323" };
			CoursesController controller = new CoursesController(mock.Object);

			var result = controller.PostCourse(newCourse);




		}

		[Fact]
		public void GetAllCoursesTest()
		{
			var mock = new Mock<ICourseService>();
			mock.Setup(repo => repo.GetAllCoursesAsync()).Returns(courses);
			CoursesController controller = new CoursesController(mock.Object);

			var GetResult = controller.GetAllCourses();

			Assert.NotNull(GetResult);
		}

		//[Fact]
		//public void PutCourseTest()
		//{

		//}

		Task<List<CourseModel>> courses = new Task<List<CourseModel>>(() => new List<CourseModel>
			{
				new CourseModel{ Id = 1, Name = "Test1"},
				new CourseModel{ Id = 2, Name = "Test2"},
				new CourseModel{ Id = 3, Name = "Test3"}
			});
	}
}