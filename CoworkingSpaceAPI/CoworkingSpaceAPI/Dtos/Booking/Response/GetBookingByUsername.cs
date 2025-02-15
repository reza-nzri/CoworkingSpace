namespace CoworkingSpaceAPI.Dtos.Booking.Response
{
    public class GetBookingByUsername
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int? DeskId { get; set; }
        public string DeskName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsCancelled { get; set; }
        public string? CancellationReason { get; set; }
        public bool IsCheckedIn { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public string Website { get; set; }
    }
}