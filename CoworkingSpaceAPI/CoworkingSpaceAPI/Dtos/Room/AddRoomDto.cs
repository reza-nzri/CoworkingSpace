namespace CoworkingSpaceAPI.Dtos.Room
{
    public class AddRoomDto
    {
        public string RoomName { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Currency { get; set; } = "EUR";  // Default value
        public bool? IsActive { get; set; } = true;
    }
}