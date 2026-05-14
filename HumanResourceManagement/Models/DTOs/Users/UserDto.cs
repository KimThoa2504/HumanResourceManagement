using static HumanResourceManagement.Models.Users.User;

namespace HumanResourceManagement.Models.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }  
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public UserStatus Status { get; set; }
    }
}
