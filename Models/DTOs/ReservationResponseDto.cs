namespace WebApi.Models.DTOs
{
    public class ReservationResponseDto
    {
        public int Id { get; set; }
        public required string RoomName { get; set; }
        public required string UserName { get; set; }
        public required string Purpose { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ReservationStatus Status { get; set; }
        public required string ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}