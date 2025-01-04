namespace CoworkingSpaceAPI.Dtos.Company.Request
{
    public class AddEmployeeDto
    {
        public string EmployeeUsername { get; set; }
        public string Position { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}