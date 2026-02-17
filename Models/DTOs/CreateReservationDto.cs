using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTOs
{
    public class CreateReservationDto
    {
        [Required]
        public int RoomId { get; set; }
        
        [Required]
        public int UserId { get; set; }

        [Required]
        public required string Purpose { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}