using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace E_Commerce.Infrastructure.Services
{
    internal class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        public TokenService(IOptions<JwtSettings>jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }
        public string GenerateToken(string userId, string email, string username, IReadOnlyList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier , userId),
                new Claim(ClaimTypes.Email , email),
                new Claim(ClaimTypes.Name , username)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role , r)));

            var secretKey = _jwtSettings.SecretKey;
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new InvalidOperationException("JWT secret key is missing");

            if(secretKey.Length < 32)
                throw new InvalidOperationException("JWT secret key is too short");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);



            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires:DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes) ,
                signingCredentials:credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
    public class JwtSettings
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpirationMinutes { get; set; }
    }
}
