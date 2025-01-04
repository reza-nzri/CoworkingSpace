namespace CoworkingSpaceAPI.Dtos.Desk.Request
{
    public class UpdateDeskDto
    {
        public string DeskName { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public bool? IsAvailable { get; set; }
        public int? NewRoomId { get; set; }  // Optional, for moving desk to another room
    }
}