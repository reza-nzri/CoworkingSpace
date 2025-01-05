namespace CoworkingSpaceAPI.Dtos.Booking.Request
{
    public class CreateBookingDto
    {
        public int CompanyId { get; set; }
        public int RoomId { get; set; }
        public int? DeskId { get; set; }  // Optional
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}