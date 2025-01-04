namespace CoworkingSpaceAPI.Dtos.Desk.Response
{
    public class DeskDetailsDto
    {
        public int DeskId { get; set; }
        public string DeskName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "EUR";
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}