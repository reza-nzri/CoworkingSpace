namespace CoworkingSpaceAPI.Dtos.Auth.Response
{
    public class UserDetailsDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? Nickname { get; set; }
        public string? RecoveryEmail { get; set; }
        public string? AlternaiveEmail { get; set; }
        public string? RecoveryPhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateOnly? Birthday { get; set; }
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
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? AddressType { get; set; }
        public bool? IsDefaultAddress { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool? LockoutEnabled { get; set; }
        public int? AccessFailedCount { get; set; }
        public List<string>? Roles { get; set; }
    }
}