using System.Collections.Generic;
using CharlieBackend.Business.Services.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.DTO.Course;

namespace CharlieBackend.Api.Controllers.Tests
{
    public class CoursesControllerUnitTest
	{
		[Fact]
		public async Task GetAllCoursesTestAsync()
		{
			//Arrange

			var courceServiceMock = new Mock<ICourseService>();
			courceServiceMock.Setup(repo => repo.GetAllCoursesAsync()).Returns(GetCourses);
			CoursesController controller = new CoursesController(courceServiceMock.Object);

			//Act

			var GetResult = controller.GetAllCourses();
			var objectResult = GetResult.Result.Result as ObjectResult;
			var toCompare = objectResult.Value as List<CourseDto>;
			var actualResult = await GetCourses();

			//Assert

			Assert.Equal(toCompare.Count, actualResult.Count);
		}

		public async Task<IList<CourseDto>> GetCourses()
		{
			List<CourseDto> coursesL = new List<CourseDto>
			{
				new CourseDto { Id = 12, Name = "Charli" },
				new CourseDto { Id = 13, Name = "Alfa" },
				new CourseDto { Id = 14, Name = "Omega" }
			};
			return coursesL;
		}	
	}
}