using WebApi.Models;
namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }

        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
    public enum UserRole
    {
        User,
        Admin
    }
}
