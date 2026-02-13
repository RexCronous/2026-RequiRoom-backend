namespace WebApi.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public required string Purpose { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ReservationStatus Status { get; set; }
        public int? ApproverId { get; set; }
        public DateTime ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum ReservationStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }
}