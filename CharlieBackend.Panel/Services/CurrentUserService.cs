using CharlieBackend.Core.Entities;
using CharlieBackend.Panel.Helpers;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private long _accountId;
        private long _entityId;
        private string _email;

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
                    var accountIdString = GetClaimValue(claimType: ClaimConstants.AccountClaim);

                    if (!long.TryParse(accountIdString, out _accountId))
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
                    //var enti =
                        //User//.Claims.FirstOrDefault(c => c.Type == ClaimConstants.IdClaim).Value;

                    var entityIdString = GetClaimValue(claimType: ClaimConstants.IdClaim);

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

        public UserRole Role
        {
            get
            {
                var roleString = GetClaimValue(claimType: ClaimsIdentity.DefaultRoleClaimType);

                UserRole role;

                if (!Enum.TryParse<UserRole>(roleString, out role))
                {
                    throw new UnauthorizedAccessException("Not authorized!");
                }

                return role;
            }
        }

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
