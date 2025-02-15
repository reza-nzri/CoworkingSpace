namespace CoworkingSpaceAPI.Dtos.Booking.Request
{
    public class BookingDurationRequestDto
    {
        public int CompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}