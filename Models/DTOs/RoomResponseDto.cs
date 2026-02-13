namespace WebApi.Models.DTOs
{
    public class RoomResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Capacity { get; set; }
        public required string Location { get; set; }
        public bool IsAvailable { get; set; }
    }
}