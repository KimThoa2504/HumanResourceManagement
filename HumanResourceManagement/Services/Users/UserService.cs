using BCrypt.Net;
using HumanResourceManagement.Data;
using HumanResourceManagement.Exceptions;
using HumanResourceManagement.Models.DTOs.Users;
using HumanResourceManagement.Models.Users;
using Microsoft.EntityFrameworkCore;
using static HumanResourceManagement.Models.Users.User;

namespace HumanResourceManagement.Services.Users
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<UserDto> CreateUser(CreateUserDto dto)
        //{
        //    if (_context.Users.Any(x => x.Username == dto.Username))
        //        throw new Exception("Username already exists");

        //    var user = new User
        //    {
        //        Username = dto.Username,
        //        Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        //        Email = dto.Email,
        //        Role = dto.Role,
        //        Status = UserStatus.ACTIVE
        //    };

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return new UserDto
        //    {
        //        Id = user.Id,
        //        Username = user.Username,
        //        Email = user.Email,
        //        Role = user.Role,
        //        Status = user.Status
        //    };
        //}
        public async Task<UserDto> CreateUser(CreateUserDto dto)
        {
            // Validate input
            if (dto == null)
                throw new ApiException("Request body is null");

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ApiException("Username is required");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ApiException("Password is required");

            //  Check duplicate username
            if (await _context.Users.AnyAsync(x => x.Username == dto.Username))
                throw new ApiException("Username already exists");

            //  Check duplicate email (nếu có)
            if (!string.IsNullOrEmpty(dto.Email))
            {
                if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
                    throw new ApiException("Email already exists");
            }

            // Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username.Trim(),
                Password = hashedPassword,
                Email = dto.Email,
                Role = dto.Role,
                Status = UserStatus.ACTIVE,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            try
            {
                // Save DB
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                //  LẤY LỖI THẬT TỪ MYSQL
                var message = ex.InnerException?.Message ?? ex.Message;

                //  Xử lý lỗi phổ biến
                if (message.Contains("Duplicate"))
                {
                    if (message.Contains("username"))
                        throw new ApiException("Username already exists");

                    if (message.Contains("email"))
                        throw new ApiException("Email already exists");
                }

                if (message.Contains("Data truncated"))
                {
                    throw new ApiException("Invalid ENUM value (Role/Status)");
                }

                // fallback
                throw new ApiException("Database error: " + message);
            }

            // 🎯 6. Return DTO
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Status = user.Status
            };
        }
        public List<UserDto> GetAll()
        {
            return _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                Status = u.Status
            }).ToList();
        }

        public async Task LockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new ApiException("User not found");

            user.Status = UserStatus.LOCKED;
            await _context.SaveChangesAsync();
        }
    }
}