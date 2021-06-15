using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class ThemeServiceTests : TestBase
    {
        private readonly Mock<IThemeRepository> _themeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public ThemeServiceTests()
        {
            _notificationServiceMock = new Mock<INotificationService>();
            _mapper = GetMapper(new ModelMappingProfile());
        }

        [Fact]
        public async Task CreateThemeAsync()
        {
            //Arrange
            var newTheme = new CreateThemeDto()
            {
                Name = "NewName"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();

            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var themeService = new ThemeService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
            var createNewResult = await themeService.CreateThemeAsync(newTheme);

            var createNullResult = await themeService.CreateThemeAsync(null);

            //Assert
            Assert.NotNull(createNewResult.Data);
            Assert.Equal(newTheme.Name, createNewResult.Data.Name);
            Assert.Equal(ErrorCode.InternalServerError, createNullResult.Error.Code);
        }

        [Fact]
        public async Task UpdateThemeAsync()
        {
            //Arrange
            long notExistingId = 100;

            var updateThemeDto = new UpdateThemeDto()
            {
                Name = "new_test_name"
            };

            var existingThemeDto = new UpdateThemeDto()
            {
                Name = "Test_name"
            };

            var existingTheme = new Theme()
            {
                Id = 10,
                Name = "Test_name"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();

            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(notExistingId))
                .ReturnsAsync(false);

            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existingTheme.Id))
                .ReturnsAsync(true);

            themeRepositoryMock.Setup(x => x.GetThemeByIdAsync(existingTheme.Id))
                .ReturnsAsync(existingTheme);

            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var themeService = new ThemeService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
            var successResult = await themeService.UpdateThemeAsync(existingTheme.Id, updateThemeDto);
            var themeDoesntExistResult = await themeService.UpdateThemeAsync(notExistingId, updateThemeDto);
            var updateToNullResult = await themeService.UpdateThemeAsync(existingTheme.Id,null);

            //Assert
            Assert.Equal(updateThemeDto.Name, successResult.Data.Name);
            Assert.Equal(ErrorCode.NotFound, themeDoesntExistResult.Error.Code);
            Assert.Equal(ErrorCode.NotFound, updateToNullResult.Error.Code);
        }

        [Fact]
        public async Task DeleteThemeAsync()
        {
            //Arrange
            long notExistingId = 100;

            var existingTheme = new Theme()
            {
                Id = 10,
                Name = "Test_name"
            };

            var expectedTheme = new ThemeDto()
            {
                Name = "Test_name"
            };

            var existingUsedTheme = new Theme()
            {
                Id = 15,
                Name = "Test_name2"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();

            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(notExistingId))
                .ReturnsAsync(false);

            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existingTheme.Id))
                .ReturnsAsync(true);

            themeRepositoryMock.Setup(x => x.IsThemeUsed(existingTheme.Id))
                .ReturnsAsync(false);

            themeRepositoryMock.Setup(x => x.GetByIdAsync(existingTheme.Id))
                .ReturnsAsync(existingTheme);

            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existingUsedTheme.Id))
                .ReturnsAsync(true);

            themeRepositoryMock.Setup(x => x.IsThemeUsed(existingUsedTheme.Id))
                .ReturnsAsync(true);

            themeRepositoryMock.Setup(x => x.GetByIdAsync(existingUsedTheme.Id))
                .ReturnsAsync(existingUsedTheme);

            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var themeService = new ThemeService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
            var successResult = await themeService.DeleteThemeAsync(existingTheme.Id);
            var themeIsUsedExistResult = await themeService.DeleteThemeAsync(existingUsedTheme.Id);
            var themeDoesntExistResult = await themeService.DeleteThemeAsync(notExistingId);

            //Assert
            Assert.Equal(expectedTheme.Name, successResult.Data.Name);
            Assert.Equal(ErrorCode.ValidationError, themeIsUsedExistResult.Error.Code);
            Assert.Equal(ErrorCode.NotFound, themeDoesntExistResult.Error.Code);
        }

        [Fact]
        public async Task GetAllThemesAsync()
        {
            //Arrange
            var allExistingThemes = new List<Theme>()
            {
                new Theme()
                {
                    Id = 145,
                    Name = "Existing_Theme"
                },
                new Theme()
                {
                    Id = 123,
                    Name = "Existing_Theme_2"
                }
            };

            var expectedResult = new List<ThemeDto>()
            {
                new ThemeDto()
                {
                    Id = 145,
                    Name = "Existing_Theme"
                },
                new ThemeDto()
                {
                    Id = 123,
                    Name = "Existing_Theme_2"
                }
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();

            themeRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(allExistingThemes);

            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var themeService = new ThemeService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
            var successResult = await themeService.GetAllThemesAsync();

            //Assert
            successResult.Data.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetThemeByNameAsync()
        {
            //Arrange
            var existingTheme = new Theme()
            {
                Id = 10,
                Name = "Test_name"
            };

            var expectedThemeDto = new ThemeDto()
            {
                Id = 10,
                Name = "Test_name"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();

            themeRepositoryMock.Setup(x => x.GetThemeByNameAsync("Test_name"))
                .ReturnsAsync(existingTheme);

            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var themeService = new ThemeService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
            var successResult = await themeService.GetThemeByNameAsync("Test_name");
            var themeDoesntExistResult = await themeService.GetThemeByNameAsync("Theme_that_doesnt_exist");

            //Assert
            Assert.Equal(expectedThemeDto.Name, successResult.Data.Name);
            Assert.Equal(expectedThemeDto.Id, successResult.Data.Id);
            Assert.Equal(ErrorCode.NotFound, themeDoesntExistResult.Error.Code);
        }
    }
}
