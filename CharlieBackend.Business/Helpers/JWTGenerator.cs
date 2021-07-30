using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
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
        public Dictionary<string, string> GetRoleJwtDictionary(AccountDto account)
        {
            var jwtDictionary = new Dictionary<string, string>();

            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (account.Role.HasFlag(role))
                {
                    jwtDictionary.Add(UserRole.Student.ToString(), GenerateEncodedJwt(account, UserRole.Student));
                }
            }

            return jwtDictionary;
        }

        private string GenerateEncodedJwt(AccountDto account, UserRole role)
        {

            var jwt = GenerateJwt(account, role);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return "Bearer " + encodedJwt; 
        }

        private JwtSecurityToken GenerateJwt(AccountDto account, UserRole role)
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
                            new Claim(ClaimConstants.IdClaim, account.Id.ToString()),
                            new Claim(ClaimConstants.EmailClaim, account.Email),
                            new Claim(ClaimConstants.AccountClaim, account.Id.ToString())
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
