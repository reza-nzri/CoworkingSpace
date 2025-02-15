namespace CoworkingSpaceAPI.Models;

public partial class AddressType
{
    public int AddressTypeId { get; set; }

    public string? AddressTypeName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<CompanyAddress> CompanyAddresses { get; set; } = new List<CompanyAddress>();

    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
}