namespace CoworkingSpaceAPI.Dtos.Booking.Response
{
    public class NoShowBookingDto
    {
        public int BookingId { get; set; }
        public string RoomName { get; set; }
        public int? DeskId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsCancelled { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}