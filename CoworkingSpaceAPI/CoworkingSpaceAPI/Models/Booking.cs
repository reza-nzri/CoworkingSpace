namespace CoworkingSpaceAPI.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public string UserId { get; set; }

    public int DeskId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal TotalCost { get; set; }

    public bool IsCancelled { get; set; }

    public string? CancellationReason { get; set; }

    public bool IsCheckedIn { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Desk Desk { get; set; } = null!;

    public virtual ApplicationUserModel User { get; set; } = null!;
}