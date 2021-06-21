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
        public async Task CreateThemeAsync_NewTheme_ShouldReturnTheme()
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

            //Assert
            createNewResult.Data.Should().NotBeNull();
            createNewResult.Data.Name.Should().BeEquivalentTo(newTheme.Name);
        }

        [Fact]
        public async Task CreateThemeAsync_Null_ShouldReturnInternalServerError()
        {
            //Arrange
            var themeRepositoryMock = new Mock<IThemeRepository>();

            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var themeService = new ThemeService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
            var createNullResult = await themeService.CreateThemeAsync(null);

            //Assert
            createNullResult.Error.Code.Should().BeEquivalentTo(ErrorCode.InternalServerError);
        }

        [Fact]
        public async Task UpdateThemeAsync_ExsistingIdNotNullData_ShouldReturnTheme()
        {
            //Arrange
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

            //Assert
            successResult.Data.Should().NotBeNull();
            successResult.Data.Name.Should().BeEquivalentTo(updateThemeDto.Name);
        }

        [Fact]
        public async Task UpdateThemeAsync_ExsistingIdNullData_ShouldReturnNotFound()
        {
            //Arrange
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

            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existingTheme.Id))
                .ReturnsAsync(true);

            themeRepositoryMock.Setup(x => x.GetThemeByIdAsync(existingTheme.Id))
                .ReturnsAsync(existingTheme);

            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var themeService = new ThemeService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
            var updateToNullResult = await themeService.UpdateThemeAsync(existingTheme.Id, null);

            //Assert
            updateToNullResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task UpdateThemeAsync_NotExistingId_ShouldReturnNotFound()
        {
            //Arrange
            long notExistingId = 100;

            var updateThemeDto = new UpdateThemeDto()
            {
                Name = "new_test_name"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();

            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(notExistingId))
                .ReturnsAsync(false);

            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var themeService = new ThemeService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
            var themeDoesntExistResult = await themeService.UpdateThemeAsync(notExistingId, updateThemeDto);

            //Assert
            themeDoesntExistResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task DeleteThemeAsync_ExistingThemeNotUsed_ShouldReturnTheme()
        {
            //Arrange
            var existingTheme = new Theme()
            {
                Id = 10,
                Name = "Test_name"
            };

            var expectedTheme = new ThemeDto()
            {
                Name = "Test_name"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();

            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existingTheme.Id))
                .ReturnsAsync(true);

            themeRepositoryMock.Setup(x => x.IsThemeUsed(existingTheme.Id))
                .ReturnsAsync(false);

            themeRepositoryMock.Setup(x => x.GetByIdAsync(existingTheme.Id))
                .ReturnsAsync(existingTheme);

            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var themeService = new ThemeService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
            var successResult = await themeService.DeleteThemeAsync(existingTheme.Id);

            //Assert
            successResult.Data.Name.Should().BeEquivalentTo(expectedTheme.Name);
        }

        [Fact]
        public async Task DeleteThemeAsync_ExistingThemeIsUsed_ShouldReturnValidationError()
        {
            //Arrange
            var existingUsedTheme = new Theme()
            {
                Id = 15,
                Name = "Test_name2"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();

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
            var themeIsUsedExistResult = await themeService.DeleteThemeAsync(existingUsedTheme.Id);

            //Assert
            themeIsUsedExistResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task DeleteThemeAsync_NotExistingThemeNotUsed_ShouldReturnNotFound()
        {
            //Arrange
            long notExistingId = 100;

            var themeRepositoryMock = new Mock<IThemeRepository>();

            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(notExistingId))
                .ReturnsAsync(false);

            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var themeService = new ThemeService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
            var themeDoesntExistResult = await themeService.DeleteThemeAsync(notExistingId);

            //Assert
            themeDoesntExistResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task GetAllThemesAsync_ValidDataPassed_ShouldReturnListOfThemes()
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
        public async Task GetThemeByNameAsync_ExistingName_ShouldReturnTheme()
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

            //Assert
            successResult.Data.Should().BeEquivalentTo(expectedThemeDto);
        }

        [Fact]
        public async Task GetThemeByNameAsync_NotExistingName_ShouldReturnNotFound()
        {
            //Arrange
            var themeRepositoryMock = new Mock<IThemeRepository>();

            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var themeService = new ThemeService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
            var themeDoesntExistResult = await themeService.GetThemeByNameAsync("Theme_that_doesnt_exist");

            //Assert
            themeDoesntExistResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }
    }
}
