namespace CoworkingSpaceAPI.Dtos.Booking.Response
{
    public class RevenueBreakdownDto
    {
        public decimal TotalRevenue { get; set; }
        public Dictionary<string, decimal> RevenueByRoom { get; set; }
        public Dictionary<string, decimal> RevenueByDesk { get; set; }
    }
}