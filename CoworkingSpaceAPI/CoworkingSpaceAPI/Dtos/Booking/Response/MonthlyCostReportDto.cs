namespace CoworkingSpaceAPI.Dtos.Booking.Response
{
    public class MonthlyCostReportDto
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int? DeskId { get; set; }
        public string? DeskName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}