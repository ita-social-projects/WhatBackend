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
		[Fact]
		public async Task GetAllCoursesTestAsync()
		{
			//Arrange

			var courceServiceMock = new Mock<ICourseService>();
			courceServiceMock.Setup(repo => repo.GetAllCoursesAsync()).Returns(getCourses);
			CoursesController controller = new CoursesController(courceServiceMock.Object);

			//Act

			var GetResult = controller.GetAllCourses();
			var objectResult = GetResult.Result.Result as ObjectResult;
			var toCompare = objectResult.Value as List<CourseModel>;
			var actualResult = await getCourses();

			//Assert

			Assert.Equal(toCompare.Count, actualResult.Count);
		}

		public async Task<IList<CourseModel>> getCourses()
		{
			List<CourseModel> coursesL = new List<CourseModel>
			{
				new CourseModel { Id = 12, Name = "Charli" },
				new CourseModel { Id = 13, Name = "Alfa" },
				new CourseModel { Id = 14, Name = "Omega" }
			};
			return coursesL;
		}	
	}
}