using System;
using System.Collections.Generic;

namespace CoworkingSpaceAPI.Models;

public partial class Desk
{
    public int DeskId { get; set; }

    public int RoomId { get; set; }

    public string DeskName { get; set; } = null!;

    public decimal? Price { get; set; }

    public string? Currency { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Room Room { get; set; } = null!;
}
