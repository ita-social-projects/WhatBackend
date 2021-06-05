using Moq;
using Xunit;
using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using CharlieBackend.Core.Entities;
using CharlieBackend.Business.Services;


namespace CharlieBackend.Api.UnitTest
{
    public class CurrentUserTests : TestBase
    {
        [Fact]
        public void ThrowErrorIfNotLoggined()
        {
            var _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessorMock.Setup(req => req.HttpContext)
                .Returns(new DefaultHttpContext());
            var currentuserService = new CurrentUserService(_httpContextAccessorMock.Object);
            Assert.Throws<UnauthorizedAccessException>(() => currentuserService.AccountId);
        }

        [Fact]
        public void getAccountId()
        {
            var _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            var mockIdentity = new Mock<GenericIdentity>("Vasya");
            mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[]{
                    new Claim("AccountId", "123") 
                });
            httpContext.User = new ClaimsPrincipal(mockIdentity.Object);
            _httpContextAccessorMock.Setup(req => req.HttpContext).Returns(httpContext);
            var currentuserService = new CurrentUserService(_httpContextAccessorMock.Object);
            var Id = currentuserService.AccountId;

            Assert.Equal(123,Id);
        }
        [Fact]
        public void getEntityId()
        {
            var _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            var mockIdentity = new Mock<GenericIdentity>("Vasya");
            mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[] { 
                    new Claim("Id", "123") 
                });
            httpContext.User = new ClaimsPrincipal(mockIdentity.Object);
            _httpContextAccessorMock.Setup(req => req.HttpContext).Returns(httpContext);

            var currentuserService = new CurrentUserService(_httpContextAccessorMock.Object);
            var EntityId = currentuserService.EntityId;

            Assert.Equal(123, EntityId);
        }
        [Fact]
        public void getEmail()
        {
            var _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();

            var mockIdentity = new Mock<GenericIdentity>("Vasya");
            mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[] {
                    new Claim("Email", "Hello@mail.com") 
                });
            httpContext.User = new ClaimsPrincipal(mockIdentity.Object);
            _httpContextAccessorMock.Setup(req => req.HttpContext).Returns(httpContext);

            var currentuserService = new CurrentUserService(_httpContextAccessorMock.Object);
            var Email = currentuserService.Email;

            Assert.Equal("Hello@mail.com", Email);
        }
        [Fact]
        public void getUserRole()
        {
            var _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            var mockIdentity = new Mock<GenericIdentity>("Vasya");
            mockIdentity.Setup(i => i.Claims)
                .Returns(new Claim[] {
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "1") 
                });
            httpContext.User = new ClaimsPrincipal(mockIdentity.Object);
            _httpContextAccessorMock.Setup(req => req.HttpContext).Returns(httpContext);
            var currentuserService = new CurrentUserService(_httpContextAccessorMock.Object);
            var Role = currentuserService.Role;

            Assert.Equal(UserRole.Student, Role);
        }
    }
}
