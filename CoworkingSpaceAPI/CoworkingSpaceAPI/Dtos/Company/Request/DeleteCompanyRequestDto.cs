namespace CoworkingSpaceAPI.Dtos.Company.Request
{
    public class DeleteCompanyRequestDto
    {
        public string Name { get; set; }
        public string Industry { get; set; }
        public DateOnly FoundedDate { get; set; }
        public string RegistrationNumber { get; set; }
        public string TaxId { get; set; }
    }
}