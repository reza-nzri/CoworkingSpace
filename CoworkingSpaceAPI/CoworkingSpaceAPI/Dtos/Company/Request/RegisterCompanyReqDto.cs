namespace CoworkingSpaceAPI.Dtos.Company.Request;

public class RegisterCompanyReqDto
{
    public string Name { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? TaxId { get; set; }
    public string? Website { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public DateOnly? FoundedDate { get; set; }
    public DateOnly StartDate { get; set; }
}