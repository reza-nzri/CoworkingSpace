namespace CoworkingSpaceAPI.Dtos.Booking.Request
{
    public class UpdateBookingDto
    {
        public int BookingId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsCancelled { get; set; }
        public string? CancellationReason { get; set; }
        public bool? IsCheckedIn { get; set; }
    }
}