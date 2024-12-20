namespace CoworkingSpaceAPI.Dtos.CEO
{
    namespace CoworkingSpaceAPI.Dtos.Company.Response
    {
        public class CompanyCeoDto
        {
            public int CompanyId { get; set; }
            public string CeoUserId { get; set; }
            public DateOnly StartDate { get; set; }
            public DateOnly? EndDate { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string? Instagram { get; set; }
        }
    }
}