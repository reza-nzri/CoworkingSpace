namespace CoworkingSpaceAPI.Dtos.Company.Request
{
    public class UpdateEmployeeDetailsDto
    {
        public string? Position { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}