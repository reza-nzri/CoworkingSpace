namespace CoworkingSpaceAPI.Dtos.Desk.Request
{
    public class AddDeskDto
    {
        public string DeskName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Currency { get; set; } = "EUR";  // Default to EUR
        public bool? IsAvailable { get; set; } = true;  // Default to true
    }
}