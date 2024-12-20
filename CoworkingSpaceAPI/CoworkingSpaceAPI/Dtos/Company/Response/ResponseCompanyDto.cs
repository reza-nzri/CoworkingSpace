namespace CoworkingSpaceAPI.Dtos.Company.Response
{
    public class ResponseCompanyDto
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string? Description { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? TaxId { get; set; }
        public string? Website { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public DateTime? FoundedDate { get; set; }
        public AddressDto Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class AddressDto
    {
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }
    }
}