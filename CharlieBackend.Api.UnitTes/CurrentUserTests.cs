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
        }

        [Fact]
        public void ThrowErrorIfNotLoggined()
        {
            _httpContextAccessorMock.Setup(req => req.HttpContext)
                .Returns(new DefaultHttpContext());

            Func<long> getId = () => _currentuserService.AccountId;
            Action act = () => getId();

            //Assert
            act.Should().Throw<UnauthorizedAccessException>();
        }

        [Fact]
        public void getAccountId()
        {
            _mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[]{
                    new Claim("AccountId", "123") 
                });
            _httpContext.User = new ClaimsPrincipal(_mockIdentity.Object);
            _httpContextAccessorMock.Setup(req => req.HttpContext).Returns(_httpContext);

            _currentuserService.AccountId.Should().Be(123);
        }
        [Fact]
        public void getEntityId()
        {
            _mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[] { 
                    new Claim("Id", "123") 
                });
            _httpContext.User = new ClaimsPrincipal(_mockIdentity.Object);
            _httpContextAccessorMock.Setup(req => req.HttpContext).Returns(_httpContext);

            //Assert
            _currentuserService.EntityId.Should().Be(123);
        }
        [Fact]
        public void getEmail()
        {

            _mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[] {
                    new Claim("Email", "Hello@mail.com") 
                });
            _httpContext.User = new ClaimsPrincipal(_mockIdentity.Object);
            _httpContextAccessorMock.Setup(req => req.HttpContext).Returns(_httpContext);


            //Assert
            _currentuserService.Email.Should().BeEquivalentTo("Hello@mail.com");
        }
        [Fact]
        public void getUserRole()
        {
            _mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[] {
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "1") 
                });
           _httpContext.User = new ClaimsPrincipal(_mockIdentity.Object);
            _httpContextAccessorMock.Setup(req => req.HttpContext).Returns(_httpContext);


            //Assert
            _currentuserService.Role.Should().BeEquivalentTo(UserRole.Student);
        }
    }
}
