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
			var themeServiceMock = new Mock<IThemeService>();
			themeServiceMock.Setup(repo => repo.GetAllThemesAsync()).Returns(getThemes);
			ThemesController controller = new ThemesController(themeServiceMock.Object);
			var GetResult = controller.GetAllThemes();
			var themesObjectResult = GetResult.Result.Result as ObjectResult;
			Assert.NotNull(themesObjectResult);
		}

		public async Task<List<ThemeModel>> getThemes()
		{
			List<ThemeModel> ThemesL = new List<ThemeModel>
			{
				new ThemeModel { Id = 12, Name = "Tema1" }
			};
			return ThemesL;
		}
	}
}
