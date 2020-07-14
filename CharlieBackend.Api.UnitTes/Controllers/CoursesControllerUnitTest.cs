using CharlieBackend.Api.Controllers;
using System.Collections.Generic;
using CharlieBackend.Business.Services.Interfaces;
using Moq;
using CharlieBackend.Core.Models.Course;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace CharlieBackend.Api.Controllers.Tests
{

	public class CoursesControllerUnitTest
	{

		//[Fact]
		//public void CoursesControllerTest()
		//{
		//}
		//[Fact]
		//public void PostCourseTest()
		//{
		//	var mock = new Mock<ICourseService>();
		//	mock.Setup(repo => repo.GetAllCoursesAsync()).Returns(courses);
		//	var newCourse = new CreateCourseModel { Id = 232, Name = "Test323" };
		//	CoursesController controller = new CoursesController(mock.Object);
		//	var result = controller.PostCourse(newCourse);
		//}

		[Fact]
		public async Task GetAllCoursesTestAsync()
		{
			var mock = new Mock<ICourseService>();
			mock.Setup(repo => repo.GetAllCoursesAsync()).Returns(courses);

			CoursesController controller = new CoursesController(mock.Object);

			var GetResult = controller.GetAllCourses();

			var a = GetResult.Result.Result as ObjectResult;
			var temp = a.Value as List<CourseModel>;

			var e = await courses();

			Assert.Equal(e.Count,temp.Count);
		}

		public async Task<List<CourseModel>> courses()
		{
			List<CourseModel> coursesL = new List<CourseModel>();
			coursesL.Add(new CourseModel { Id = 12, Name = "Charli" });
			coursesL.Add(new CourseModel { Id = 13, Name = "Alfa" });
			coursesL.Add(new CourseModel { Id = 14, Name = "Omega" });

			return coursesL;
		}	



//[Fact]
		//public void PutCourseTest()
		//{
		//}

		////var s = tr();
		////s.Start();
		////var PressF = s.Result;
		////Assert.Equal("3", PressF );
	}
}