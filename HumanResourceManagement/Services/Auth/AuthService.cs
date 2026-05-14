using HumanResourceManagement.Data;
using HumanResourceManagement.Exceptions;
using HumanResourceManagement.Models.DTOs.Login;
using HumanResourceManagement.Models.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HumanResourceManagement.Services.Auth
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public string Login(LoginDto dto)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Username == dto.Username);

            if (user == null)
                throw new ApiException("Invalid username");

            if (string.IsNullOrEmpty(user.Password))
                throw new ApiException("User password is invalid in database");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                throw new ApiException("Invalid password");

            if (user.Status == User.UserStatus.LOCKED)
                throw new ApiException("Account is locked");

            return GenerateToken(user);
        }
        private string GenerateToken(User user)
        {
            var jwt = _config.GetSection("Jwt");

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("UserId", user.Id.ToString())
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    int.Parse(jwt["ExpireMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

