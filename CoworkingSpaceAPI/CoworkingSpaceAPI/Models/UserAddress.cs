namespace CoworkingSpaceAPI.Models;

public partial class UserAddress
{
    public int UserAddressId { get; set; }

    public string? UserId { get; set; }

    public int? AddressId { get; set; }

    public int? AddressTypeId { get; set; }

    public bool? IsDefault { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Address? Address { get; set; }

    public virtual AddressType? AddressType { get; set; }

    public virtual ApplicationUserModel? User { get; set; }
}