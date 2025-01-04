namespace CoworkingSpaceAPI.Dtos.Company.Response
{
    public class CompanyDetailsDto
    {
        // Company information
        public int CompanyId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Industry { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public DateOnly? FoundedDate { get; set; }

        // Address information
        public string Street { get; set; } = string.Empty;

        public string HouseNumber { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string TypeDescription { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public DateTime? UpdatedAt { get; set; }

        // CEO information
        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
        public string CeoUsername { get; set; } = string.Empty;
    }
}