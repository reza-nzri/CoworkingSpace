namespace CoworkingSpaceAPI.Dtos.Booking.Response
{
    namespace CoworkingSpaceAPI.Dtos.Booking
    {
        public class OccupancyReportDto
        {
            public double OccupancyRate { get; set; }
            public int TotalBookings { get; set; }
            public RoomOccupancyDto MostUsedRoom { get; set; }
            public DeskOccupancyDto LeastUsedDesk { get; set; }
            public string Currency { get; set; } = "EUR";
        }

        public class RoomOccupancyDto
        {
            public int RoomId { get; set; }
            public string RoomName { get; set; }
            public double OccupancyPercentage { get; set; }
        }

        public class DeskOccupancyDto
        {
            public int DeskId { get; set; }
            public string DeskName { get; set; }
            public double OccupancyPercentage { get; set; }
        }
    }
}