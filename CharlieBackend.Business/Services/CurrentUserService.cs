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
        readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long AccountId
        {
            get
            {
                string accountIdString = _httpContextAccessor
                    .HttpContext
                    .User
                    ?.Claims
                    ?.SingleOrDefault(claim => claim.Type == "AccountId")
                    ?.Value;

                long accountId;

                if (!long.TryParse(accountIdString, out accountId))
                {
                    throw new UnauthorizedAccessException("Not authorized!");
                }

                return accountId;
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
                string entityIdString = _httpContextAccessor
                    .HttpContext
                    .User
                    ?.Claims
                    ?.SingleOrDefault(claim => claim.Type == "Id")
                    ?.Value;

                long entityId;

                if (!long.TryParse(entityIdString, out entityId))
                {
                    throw new UnauthorizedAccessException("Not authorized!");
                }

                return entityId;
            }
        }

        public string Email
        {
            get
            {
                string email = _httpContextAccessor
                    .HttpContext
                    .User
                    ?.Claims
                    ?.SingleOrDefault(claim => claim.Type == "Email")
                    ?.Value;

                if (email is null)
                {
                    throw new UnauthorizedAccessException("Not authorized!");
                }

                return email;
            }
        }

        public UserRole Role
        {
            get
            {
                string roleString = _httpContextAccessor
                    .HttpContext
                    .User
                    ?.Claims
                    ?.SingleOrDefault(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType)
                    ?.Value;

                UserRole role;

                if (!Enum.TryParse<UserRole>(roleString, out role))
                {
                    throw new UnauthorizedAccessException("Not authorized!");
                }

                return role;
            }
        }
    }
}
