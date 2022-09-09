using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CharlieBackend.Business.Options;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Business.Helpers
{
    public class JwtGenerator : IJwtGenerator
    {
        private AuthOptions _authOptions;

        public JwtGenerator(IOptions<AuthOptions> authOptions)
        {
            _authOptions = authOptions.Value;
        }
        public Dictionary<string, string> GetRoleJwtDictionary(AccountDto account, Dictionary<UserRole, long> roleIds)
        {
            var jwtDictionary = new Dictionary<string, string>();

            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (role == UserRole.NotAssigned)
                {
                    continue;
                }

                if (account.Role.HasFlag(role) )
                {
                    jwtDictionary.Add(role.ToString(), GenerateEncodedJwt(account, role, roleIds[role]));
                }
            }

            return jwtDictionary;
        }

        public string GenerateEncodedJwt(AccountDto account, UserRole role, long roleId)
        {

            var jwt = GenerateJwt(account, role, roleId);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return "Bearer " + encodedJwt; 
        }

        private JwtSecurityToken GenerateJwt(AccountDto account, UserRole role, long roleId)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                        issuer: _authOptions.ISSUER,
                        audience: _authOptions.AUDIENCE,
                        notBefore: now,
                        claims: new List<Claim>
                        {
                            new Claim(ClaimsIdentity.DefaultRoleClaimType,
                                    role.ToString()),
                            new Claim(ClaimConstants.IdClaim, roleId.ToString()),
                            new Claim(ClaimConstants.EmailClaim, account.Email),
                            new Claim(ClaimConstants.AccountClaim, account.Id.ToString()),
                            new Claim(ClaimConstants.FirstName, account.FirstName),
                            new Claim(ClaimConstants.LastName, account.LastName),
                            new Claim(ClaimConstants.Localization, account.Localization)
                        },
                        expires: now.Add(TimeSpan.FromMinutes(_authOptions.LIFETIME)),
                        signingCredentials:
                                new SigningCredentials(
                                        _authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                        );

            return jwt;
        }
    }
}
