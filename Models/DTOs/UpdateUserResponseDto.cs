namespace WebApi.Models.DTOs
{
    public class UpdateUserResponseDto
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public UserRole Role { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}