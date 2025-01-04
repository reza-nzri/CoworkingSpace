namespace CoworkingSpaceAPI.Dtos.Company.Request
{
    public class DeleteEmployeeRequestDto
    {
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public DateOnly FoundedDate { get; set; }
        public string RegistrationNumber { get; set; }
        public string TaxId { get; set; }
        public string EmployeeUsername { get; set; }
    }
}