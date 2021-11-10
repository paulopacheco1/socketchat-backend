using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocketChat.Domain.Entities;
using SocketChat.Domain.Providers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocketChat.Infrastructure.Auth
{
    public sealed class AuthService : IAuthServiceProvider
    {
        private readonly JwtOptions _jwtOptions;
        public AuthService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string CriarTokenJwt(Usuario usuario)
        {
            var accessSecret = Convert.FromBase64String(_jwtOptions.AccessSecret);
            var accessKey = new SymmetricSecurityKey(accessSecret);
            var credentials = new SigningCredentials(accessKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                expires: DateTime.Now.AddMinutes(_jwtOptions.AccessValidFor),
                signingCredentials: credentials,
                claims: claims
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}