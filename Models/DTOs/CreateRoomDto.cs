using System.ComponentModel.DataAnnotations;
namespace WebApi.Models.DTOs
{
    public class CreateRoomDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Capacity { get; set; }

        [Required]
        public required string Location { get; set; }
    }
}