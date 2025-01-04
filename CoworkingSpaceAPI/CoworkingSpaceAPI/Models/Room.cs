namespace CoworkingSpaceAPI.Models;

public partial class Room
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = null!;
    public string? RoomType { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CompanyAddressId { get; set; }
    public virtual CompanyAddress? CompanyAddress { get; set; }
    public virtual ICollection<Desk> Desks { get; set; } = new List<Desk>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}