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
        private string _firstName;
        private string _lastName;
        private string _localization;

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

        public string FirstName
        {
            get
            {
                if (_firstName is null)
                {
                    _firstName = GetClaimValue(ClaimsConstants.FirstName);
                    if (_firstName is null)
                    {
                        throw new UnauthorizedAccessException("Not authorized!");
                    }
                }
                return _firstName;
            }
        }

        public string LastName
        {
            get
            {
                if (_lastName is null)
                {
                    _lastName = GetClaimValue(ClaimsConstants.LastName);
                    if (_lastName is null)
                    {
                        throw new UnauthorizedAccessException("Not authorized!");
                    }
                }
                return _lastName;
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
   
        public string Localization
        {
            get
            {
                if (_localization is null)
                {
                    _localization = GetClaimValue(ClaimsConstants.Localization);
                    if (_localization is null)
                    {
                        throw new UnauthorizedAccessException("Not authorized!");
                    }
                }
                return _localization;
            }
            set
            {
                _localization = value;
                ChangeClaimValue(ClaimsConstants.Localization, _localization);
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

        private void ChangeClaimValue(string claimType, string updatedValue)
        {
            var Identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            Identity.RemoveClaim(Identity.FindFirst(claimType));
            Identity.AddClaim(new Claim(claimType, updatedValue));
        }
    }
}
