namespace CoworkingSpaceAPI.Dtos.Company.Response
{
    public class AdminCompanyDto
    {
        // Company Info
        public string Name { get; set; }

        public string Industry { get; set; }
        public string Description { get; set; }
        public string RegistrationNumber { get; set; }
        public string TaxId { get; set; }
        public string Website { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public DateOnly? FoundedDate { get; set; }

        // CEO Info
        public string CeoUsername { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        // Address Info
        public List<AdminCompanyAddressDto> Addresses { get; set; }
    }

    public class AdminCompanyAddressDto
    {
        public bool IsDefault { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string AddressType { get; set; }
    }
}