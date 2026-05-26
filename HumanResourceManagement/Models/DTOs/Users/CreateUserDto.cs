using static HumanResourceManagement.Models.Users.User;

namespace HumanResourceManagement.Models.DTOs.Users
{
    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; }
        public UserRole Role { get; set; } = UserRole.VIEWER;
    }
}
