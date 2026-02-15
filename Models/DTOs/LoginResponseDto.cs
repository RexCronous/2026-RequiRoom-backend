namespace WebApi.Models.DTOs
{
    public class LoginResponseDto
    {
        public required string Token { get; set; }
        public required UserResponseDto User { get; set; }
    }
}