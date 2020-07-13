using System.Collections.Generic;
using CharlieBackend.Business.Services.Interfaces;
using Moq;
using CharlieBackend.Core.Models.Course;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Microsoft.VisualBasic;

namespace CharlieBackend.Api.Controllers.Tests
{
	
	public class CoursesControllerUnitTest
	{
		

		[Fact]
		public async Task CoursesTestAsync()
		{
			var mock = new Mock<ICourseService>();
			mock.Setup(repo => repo.GetAllCoursesAsync()).Returns(courses);
			CoursesController controller = new CoursesController(mock.Object);

			var GetResult = await controller.GetAllCourses();

			Assert.NotNull(GetResult);
		}

			Task<List<CourseModel>> courses = new Task<List<CourseModel>>(()=> new List<CourseModel>
			{
				new CourseModel{ Id = 1, Name = "Test1"},
				new CourseModel{ Id = 2, Name = "Test2"},
				new CourseModel{ Id = 3, Name = "Test3"}
			});
			
		

	}
}