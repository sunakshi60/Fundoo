using BussinessLayer.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(int userId, string email)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = jwtSettings["Key"];
            var issuer = jwtSettings["Isser"];
            var audience = jwtSettings["Audience"];

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key!)
                );

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(jwtSettings["DurationInMinutes"])
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateResetToken(int userId, string email)
        {
            var claims = new[]
            {
            new Claim("UserId", userId.ToString()),
            new Claim("Email", email),
            new Claim("TokenType", "PasswordReset")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                )
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
