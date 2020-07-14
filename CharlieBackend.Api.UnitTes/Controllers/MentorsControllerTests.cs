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
			var mock = new Mock<IMentorService>();
			var mock2 = new Mock<IAccountService>();

			mock.Setup(repo => repo.GetAllMentorsAsync()).Returns(mentors);

			MentorsController controller = new MentorsController(mock.Object,mock2.Object);

			var GetResult = controller.GetAllMentors();

			var a = GetResult.Result.Result as ObjectResult;
			var z = a.Value as MentorModel;
			
			var temp = a.Value as List<MentorModel>;

			var e = await mentors();

           Assert.Equal(e.Count, temp.Count);
		}

		public async Task<List<MentorModel>> mentors()
		{
			List<MentorModel> mentorsM = new List<MentorModel>();
			mentorsM.Add(new MentorModel { Id = 10, FirstName="Hagrid", LastName="Rub", Email="hagrub@gmail.com"});
			mentorsM.Add(new MentorModel { Id = 9,FirstName="Test1",LastName="LastTest1",Email="test1@gmail.com" });
			mentorsM.Add(new MentorModel { Id = 7, FirstName="Test2", LastName = "LastTest2", Email = "test2@gmail.com" });

			return mentorsM;
		}
	}
}