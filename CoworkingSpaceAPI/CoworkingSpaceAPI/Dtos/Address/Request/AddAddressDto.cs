namespace CoworkingSpaceAPI.Dtos.Address.Request
{
    public class AddAddressDto
    {
        public string? CompanyName { get; set; } = string.Empty;
        public string? AddressTypeName { get; set; } = string.Empty;
        public string? AddressTypeDescription { get; set; }
        public string? Street { get; set; } = string.Empty;
        public string? HouseNumber { get; set; } = string.Empty;
        public string? PostalCode { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public bool? IsDefault { get; set; }
    }
}