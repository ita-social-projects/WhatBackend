using System;
using CharlieBackend.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using CharlieBackend.Business.Options;
using System.IdentityModel.Tokens.Jwt;
using CharlieBackend.Core.DTO.Account;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Api.SwaggerExamples.AccountsController;

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
                            new Claim("Id", account.Id.ToString()),
                            new Claim("Email", account.Email),
                            new Claim("AccountId", account.Id.ToString())
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
