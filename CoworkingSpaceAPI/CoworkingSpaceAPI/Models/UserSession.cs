namespace CoworkingSpaceAPI.Models;

public partial class UserSession
{
    public int SessionId { get; set; }

    public string UserId { get; set; }

    public DateTime? LoginTime { get; set; }

    public DateTime? LogoutTime { get; set; }

    public string? LastIp { get; set; }

    public string? Device { get; set; }

    public string? Browser { get; set; }

    public string? OperatingSystem { get; set; }

    public bool? IsActive { get; set; }

    public string? Location { get; set; }

    public int? LoginAttempts { get; set; }

    public int? FailedLoginAttempts { get; set; }

    public int? SessionDuration { get; set; }

    public virtual ApplicationUserModel User { get; set; } = null!;
}