namespace CoworkingSpaceAPI.Dtos.Booking.Response
{
    public class BookingStatisticsDto
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int? DeskId { get; set; }
        public string? DeskName { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}