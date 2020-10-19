using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;
using CharlieBackend.Business.Services.Interfaces;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.Models;

namespace CharlieBackend.Api.Controllers.Tests
{
    public class MentorsControllerTests
    {
		[Fact]
		public async Task GetAllMentorsTestAsync()
		{
			//Arrange

			var mentorServiceMock = new Mock<IMentorService>();
			var accountServiceMock = new Mock<IAccountService>();
			mentorServiceMock.Setup(repo => repo.GetAllMentorsAsync()).Returns(GetMentors);
			MentorsController controller = new MentorsController
			(
				mentorServiceMock.Object, 
				accountServiceMock.Object
			);

			//Act

			var GetResult = controller.GetAllMentors();
			var taskResult = GetResult.Result.Result as ObjectResult;
			var toCompare = taskResult.Value as List<MentorModel>;
			var actualResult = await GetMentors();

			//Assert

			Assert.Equal(toCompare.Count, actualResult.Count);
		}

		public async Task<IList<MentorModel>> GetMentors()
		{
			List<MentorModel> mentorsM = new List<MentorModel>() 
			{
				new MentorModel { Id = 10, FirstName="Hagrid", LastName="Rub", Email="hagrub@gmail.com"},
				new MentorModel { Id = 9, FirstName = "Test1", LastName = "LastTest1", Email = "test1@gmail.com" },
				new MentorModel { Id = 7, FirstName = "Test2", LastName = "LastTest2", Email = "test2@gmail.com" }
			};
			return mentorsM;
		}
	}
}