using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTOs
{
    public class CreateUserRequestDto
    {
        [Required]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}