using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTOs
{
    public class LoginUserRequestDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
    }
}