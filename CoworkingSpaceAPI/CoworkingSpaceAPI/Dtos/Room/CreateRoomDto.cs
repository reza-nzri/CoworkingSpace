namespace CoworkingSpaceAPI.Dtos.Room
{
    public class CreateRoomDto
    {
        public string RoomName { get; set; } = null!;
        public string? RoomType { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public string CompanyName { get; set; } = null!; // Name of the company for validation
    }
}