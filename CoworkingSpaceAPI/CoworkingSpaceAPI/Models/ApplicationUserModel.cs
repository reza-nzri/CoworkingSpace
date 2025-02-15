using Microsoft.AspNetCore.Identity;

namespace CoworkingSpaceAPI.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? Nickname { get; set; }
        public string? RecoveryEmail { get; set; }
        public string? AlternaiveEmail { get; set; }
        public string? RecoveryPhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? CompanyName { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public string? AppLanguage { get; set; }
        public string? Website { get; set; }
        public string? Linkedin { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? Github { get; set; }
        public string? Youtube { get; set; }
        public string? Tiktok { get; set; }
        public string? Snapchat { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? Status { get; set; }
        public virtual ICollection<CompanyCeo> CompanyCeos { get; set; } = new List<CompanyCeo>();
        public virtual ICollection<CompanyEmployee> CompanyEmployees { get; set; } = new List<CompanyEmployee>();
        public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
        public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}