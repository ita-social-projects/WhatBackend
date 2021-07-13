using System;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using CharlieBackend.Business.Options;
using System.IdentityModel.Tokens.Jwt;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Business.Helpers;

namespace CharlieBackend.Api.Helpers
{
    public static class JWTGenerator
    {
        public static string GenerateEncodedJWT(AuthOptions authOptions, AccountDto account)
        {
            var jwt = GenerateJWT(authOptions, account);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
        private static JwtSecurityToken GenerateJWT(AuthOptions authOptions, AccountDto account)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                        issuer: authOptions.ISSUER,
                        audience: authOptions.AUDIENCE,
                        notBefore: now,
                        claims: new List<Claim>
                        {
                            new Claim(ClaimsIdentity.DefaultRoleClaimType,
                                    account.Role.ToString()),
                            new Claim(ClaimConstants.IdClaim, account.Id.ToString()),
                            new Claim(ClaimConstants.EmailClaim, account.Email),
                            new Claim(ClaimConstants.AccountClaim, account.Id.ToString())
                        },
                        expires: now.Add(TimeSpan.FromMinutes(authOptions.LIFETIME)),
                        signingCredentials:
                                new SigningCredentials(
                                        authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                        );

            return jwt;
        }
    }
}
