namespace CoworkingSpaceAPI.Dtos.Company.Response
{
    public class EmployeeDetailsDto
    {
        public string EmployeeUsername { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}