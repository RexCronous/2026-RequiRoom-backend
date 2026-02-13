namespace WebApi.Models
{
    public class Room
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Capacity { get; set; }
        public required string Location { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}