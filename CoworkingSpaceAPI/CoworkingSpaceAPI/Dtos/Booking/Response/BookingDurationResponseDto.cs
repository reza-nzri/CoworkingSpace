namespace CoworkingSpaceAPI.Dtos.Booking.Response
{
    public class BookingDurationResponseDto
    {
        public double AverageDurationHours { get; set; }
        public int TotalBookings { get; set; }
        public int CompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}