namespace CoworkingSpaceAPI.Models;

public partial class CompanyCeo
{
    public int CompanyId { get; set; }

    public string CeoUserId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ApplicationUserModel CeoUser { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;
}