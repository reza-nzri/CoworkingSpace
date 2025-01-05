namespace CoworkingSpaceAPI.Dtos.Booking.Response
{
    public class CheckAvailabilityDeskDto
    {
        public int DeskId { get; set; }
        public string DeskName { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public bool IsAvailable { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
    }
}