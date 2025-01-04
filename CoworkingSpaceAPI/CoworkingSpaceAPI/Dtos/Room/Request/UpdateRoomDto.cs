namespace CoworkingSpaceAPI.Dtos.Room.Request
{
    public class UpdateRoomDto
    {
        public string RoomName { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
    }
}