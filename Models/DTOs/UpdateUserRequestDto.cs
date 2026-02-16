using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTOs
{
    public class UpdateUserRequestDto
    {
        public required string FullName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [MinLength(6)]
        public required string Password { get; set; }

        public UserRole Role { get; set; }
    }
}