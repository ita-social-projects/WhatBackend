using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace CharlieBackend.Business.Services
{
    /// <summary>
    /// Class that provides access to properties of the current user.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private long _accountId;
        private long _entityId;
        private string _email;
        private string _localization;

        readonly IHttpContextAccessor _httpContextAccessor;

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
                    string accountIdString = GetClaimValue(claimType: ClaimConstants.AccountClaim);

                    if (!long.TryParse(accountIdString, out _accountId))
                    {
                        throw new UnauthorizedAccessException("Not authorized!");
                    }
                }

                return _accountId;
            }
        }

        /// <summary>
        /// Property that provides access to entity id of the current user.
        /// Corresponds to one of the following entities: 
        /// <list type="bullet">
        /// <item><see cref="Student"/></item>
        /// <item><see cref="Secretary"/></item>
        /// <item><see cref="Mentor"/></item>
        /// </list>
        /// </summary>
        public long EntityId
        {
            get
            {
                if (_entityId == default)
                {
                    string entityIdString = GetClaimValue(claimType: ClaimConstants.IdClaim);

                    if (!long.TryParse(entityIdString, out _entityId))
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
                    _email = GetClaimValue(claimType: ClaimConstants.EmailClaim);

                    if (_email is null)
                    {
                        throw new UnauthorizedAccessException("Not authorized!");
                    }
                }

                return _email;
            }
        }

        public string Localization
        {            
            get
            {
                var defaultLocalization = GetClaimValue(claimType: ClaimConstants.Localization);
                if (defaultLocalization is null)
                {
                    throw new UnauthorizedAccessException("Not authorized!");
                }
                var currentCookieLocalization = _httpContextAccessor.HttpContext.Request.Cookies[ClaimConstants.Localization];
                _localization = string.IsNullOrEmpty(currentCookieLocalization)
                    ? defaultLocalization
                    : currentCookieLocalization;
                return _localization;
            }
            set
            {
                _localization = value;
                if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(ClaimConstants.Localization))
                    _httpContextAccessor.HttpContext.Response.Cookies.Delete(ClaimConstants.Localization);
                _httpContextAccessor.HttpContext.Response.Cookies.Append(ClaimConstants.Localization, _localization);
            }
        }

        public UserRole Role
        {
            get
            {
                string roleString = GetClaimValue(claimType: ClaimsIdentity.DefaultRoleClaimType);


                if (!Enum.TryParse<UserRole>(roleString, out UserRole role))
                {
                    throw new UnauthorizedAccessException("Not authorized!");
                }

                return role;
            }
        }

        /// <summary>
        /// Gets claim value by the given claim type.
        /// </summary>
        /// <returns>string representation of claim value,
        /// or null if user is not authentified</returns>
        private string GetClaimValue(string claimType)
        {
            return _httpContextAccessor
                .HttpContext
                .User
                ?.Claims
                ?.SingleOrDefault(claim => claim.Type == claimType)
                ?.Value;
        }
    }
}
