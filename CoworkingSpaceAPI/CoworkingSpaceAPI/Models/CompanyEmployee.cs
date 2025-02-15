namespace CoworkingSpaceAPI.Models;

public partial class CompanyEmployee
{
    public int CompanyId { get; set; }

    public string UserId { get; set; }

    public string? Position { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ApplicationUserModel User { get; set; } = null!;
}