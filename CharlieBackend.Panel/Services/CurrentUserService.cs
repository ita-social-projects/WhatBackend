using CharlieBackend.Core.Entities;
using CharlieBackend.Panel.Helpers;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace CharlieBackend.Panel.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private long _accountId;
        private long _entityId;
        private string _email;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long AccountId
        {
            get 
            {
                if (_accountId == default)
                {
                    var accountId = GetClaimValue(claimType: ClaimsConstants.AccountId);
                    if (!long.TryParse(accountId, out _accountId))
                    {
                        throw new UnauthorizedAccessException("Not authorized!");
                    }                    
                }

                return _accountId;
            }
        }

        public long EntityId
        {
            get
            {
                if (_entityId == default)
                {
                    var entityId = GetClaimValue(ClaimsConstants.EntityId);
                    if (!long.TryParse(entityId, out _entityId))
                    {
                        throw new UnauthorizedAccessException("Not authorized!");
                    }
                }

                return _entityId;
            }
        }

        public string Email
        {
            get
            {
                if (_email is null)
                {
                    _email = GetClaimValue(ClaimsConstants.Email);

                    if (_email is null)
                    {
                        throw new UnauthorizedAccessException("Not authorized!");
                    }
                }

                return _email;
            }
        }

        public UserRole Role
        {
            get
            {
                string role = GetClaimValue(ClaimsIdentity.DefaultRoleClaimType);

                if (!Enum.TryParse<UserRole>(role, out UserRole userRole))
                {
                    throw new UnauthorizedAccessException("Not authorized!");
                }
                return userRole;
            }
        }

        private string GetClaimValue(string claimType)
        {
            return _httpContextAccessor.HttpContext
                .User?
                .Claims?
                .SingleOrDefault(claim => claim.Type == claimType)?
                .Value;
        }
    }
}
