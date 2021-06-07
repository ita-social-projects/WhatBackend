using Moq;
using Xunit;
using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using CharlieBackend.Core.Entities;
using CharlieBackend.Business.Services;
using FluentAssertions;


namespace CharlieBackend.Api.UnitTest
{
    public class CurrentUserTests : TestBase
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly DefaultHttpContext _httpContext;
        private readonly Mock<GenericIdentity> _mockIdentity;
        private  CurrentUserService _currentuserService;

        public CurrentUserTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContext = new DefaultHttpContext();
            _mockIdentity = new Mock<GenericIdentity>("Test");
            _currentuserService = new CurrentUserService(_httpContextAccessorMock.Object);
            _httpContext.User = new ClaimsPrincipal(_mockIdentity.Object);
            _httpContextAccessorMock.Setup(req => req.HttpContext).Returns(_httpContext);
        }

        [Fact]

        public void AccountId_UserNotLoggedIn_ThrowsUnauthorizedAccessException()
        {
            _httpContextAccessorMock.Setup(req => req.HttpContext)
                .Returns(new DefaultHttpContext());

            // Act
            Func<long> act = () => _currentuserService.AccountId;

            //Assert
            act.Should().Throw<UnauthorizedAccessException>();
        }

        [Fact]

        public void GetAccountId_ValidDataPassed_ShouldReturnExpectedAccountId()
        {
            // Arrange
            long expectedAccountId = 123;

            _mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[]{
                    new Claim("AccountId", expectedAccountId.ToString()) 
                });

            // Act & Assert
            _currentuserService.AccountId.Should().Be(expectedAccountId);
        }

        [Fact]

        public void GetEntityId_ValidDataPassed_ShouldReturnExpectedEntityId()
        {
            // Arrange
            long expectedEntityId = 123;

            _mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[] { 
                    new Claim("Id", expectedEntityId.ToString()) 
                });


            // Act & Assert
            _currentuserService.EntityId.Should().Be(expectedEntityId);
        }

        [Fact]

        public void GetEmail_ValidDataPassed_ShouldReturnExpectedEmail()
        {
            // Arrange
            string expectedEmail = "Hello@mail.com";

            _mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[] {
                    new Claim("Email", expectedEmail) 
                });

            // Act & Assert
            _currentuserService.Email.Should().BeEquivalentTo(expectedEmail);
        }

        [Fact]

        public void GetUserRole_ValidDataPassed_ShouldReturnExpectedUserRole()
        {
            UserRole expectedRole = UserRole.Student;
            _mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[] {
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, expectedRole.ToString()) 
                });

            // Act & Assert
            _currentuserService.Role.Should().BeEquivalentTo(expectedRole);
        }
    }
}
