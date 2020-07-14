using CharlieBackend.Api.Controllers;
using System.Collections.Generic;
using CharlieBackend.Business.Services.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.Models.Theme;

namespace CharlieBackend.Api.UnitTest.Controllers
{
	public class ThemesControllerTests
	{
		[Fact]
		public void GetAllThemesTestAsync()
		{
			var mock = new Mock<IThemeService>();
			mock.Setup(repo => repo.GetAllThemesAsync()).Returns(Themes);

			ThemesController controller = new ThemesController(mock.Object);

			var GetResult = controller.GetAllThemes();

			var a = GetResult.Result.Result as ObjectResult;

			Assert.NotNull(a);
		}

		public async Task<List<ThemeModel>> Themes()
		{
			List<ThemeModel> ThemesL = new List<ThemeModel>();
			ThemesL.Add(new ThemeModel { Id = 12, Name = "Tema1" });
	
			return ThemesL;
		}
	}
}
