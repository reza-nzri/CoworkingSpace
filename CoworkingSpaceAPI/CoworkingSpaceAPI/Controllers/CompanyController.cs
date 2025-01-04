using AutoMapper;
using CoworkingSpaceAPI.Dtos.CEO.CoworkingSpaceAPI.Dtos.Company.Response;
using CoworkingSpaceAPI.Dtos.Company.Request;
using CoworkingSpaceAPI.Dtos.Company.Response;
using CoworkingSpaceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CoworkingSpaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CoworkingSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CompanyController(CoworkingSpaceDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/Company/admin/get-all-companies
        [HttpGet("admin/get-all-companies")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCompanies()
        {
            // Fetch all companies
            var companies = await _context.Companies
                .Include(c => c.CompanyAddresses) // Include company addresses
                    .ThenInclude(ca => ca.Address) // Include address details
                .Include(c => c.CompanyAddresses)
                    .ThenInclude(ca => ca.AddressType) // Include address type details
                .Include(c => c.CompanyCeos) // Include company CEOs
                    .ThenInclude(cc => cc.CeoUser) // Include CeoUser for CeoUsername
                .ToListAsync();

            if (!companies.Any())
            {
                return NotFound(new { StatusCode = 404, Message = "No companies found." });
            }

            // Map companies to DTOs
            var companyDetailsDtos = _mapper.Map<List<CompanyDetailsDto>>(companies);

            // Ensure CompanyId is explicitly mapped if AutoMapper fails
            foreach (var company in companies)
            {
                var dto = companyDetailsDtos.FirstOrDefault(c => c.Name == company.Name);
                if (dto != null)
                {
                    dto.CompanyId = company.CompanyId; // Direct assignment
                }
            }

            foreach (var company in companies)
            {
                var companyDto = companyDetailsDtos.FirstOrDefault(c => c.Name == company.Name);
                companyDto.CompanyId = company.CompanyId; // Explicitly assign the CompanyId

                if (companyDto != null)
                {
                    // Map CEO details if available
                    var ceo = company.CompanyCeos.FirstOrDefault();
                    if (ceo != null && ceo.CeoUser != null)
                    {
                        companyDto.CeoUsername = ceo.CeoUser.UserName;
                    }
                }
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "Companies retrieved successfully.",
                Data = companyDetailsDtos
            });
        }

        [HttpGet("ceo/get-company-details")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> GetCompanyDetails()
        {
            // Extract username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the user by username
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"User with username '{username}' was not found."
                });
            }

            // Fetch the company details associated with the user
            var companies = await _context.Companies
                .Include(c => c.CompanyAddresses)
                    .ThenInclude(ca => ca.Address)
                .Include(c => c.CompanyAddresses)
                    .ThenInclude(ca => ca.AddressType)
                .Include(c => c.CompanyCeos)
                    .ThenInclude(cc => cc.CeoUser)
                .Where(c => c.CompanyCeos.Any(cc => cc.CeoUserId == user.Id))
                .ToListAsync();

            if (companies == null || !companies.Any())
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "The user does not own any companies.",
                    Success = false,
                    Data = new List<object>()
                });
            }

            // Map the company to a DTO, including Address and AddressType manually
            var companyDetailsDtoList = companies.Select(c => new CompanyDetailsDto
            {
                CompanyId = c.CompanyId,
                Name = c.Name,
                Industry = c.Industry,
                Description = c.Description,
                RegistrationNumber = c.RegistrationNumber,
                TaxId = c.TaxId,
                Website = c.Website,
                ContactEmail = c.ContactEmail,
                ContactPhone = c.ContactPhone,
                FoundedDate = c.FoundedDate,
                CeoUsername = c.CompanyCeos.FirstOrDefault()?.CeoUser?.UserName,
                Street = c.CompanyAddresses.FirstOrDefault()?.Address?.Street ?? "",
                HouseNumber = c.CompanyAddresses.FirstOrDefault()?.Address?.HouseNumber ?? "",
                PostalCode = c.CompanyAddresses.FirstOrDefault()?.Address?.PostalCode ?? "",
                City = c.CompanyAddresses.FirstOrDefault()?.Address?.City ?? "",
                State = c.CompanyAddresses.FirstOrDefault()?.Address?.State ?? "",
                Country = c.CompanyAddresses.FirstOrDefault()?.Address?.Country ?? "",
                Type = c.CompanyAddresses.FirstOrDefault()?.AddressType?.AddressTypeName ?? "",
                TypeDescription = c.CompanyAddresses.FirstOrDefault()?.AddressType?.Description ?? "",
                CreatedAt = c.CompanyAddresses.FirstOrDefault()?.CreatedAt ?? DateTime.MinValue,
                UpdatedAt = c.CompanyAddresses.FirstOrDefault()?.UpdatedAt,
                IsDefault = c.CompanyAddresses.FirstOrDefault()?.IsDefault ?? false,
                StartDate = c.CompanyCeos.FirstOrDefault()?.StartDate ?? DateOnly.MinValue,
                EndDate = c.CompanyCeos.FirstOrDefault()?.EndDate
            }).ToList();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Company details retrieved successfully.",
                Success = true,
                Data = companyDetailsDtoList
            });
        }

        [HttpGet("ceo/get-all-employees")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> GetAllEmployees([FromQuery] int CompanyId)
        {
            // Extract CEO username from JWT
            var ceoUsername = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(ceoUsername))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the CEO user
            var ceoUser = await _userManager.FindByNameAsync(ceoUsername);
            if (ceoUser == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "CEO not found."
                });
            }

            // Find the target company by parameters
            var company = await _context.Companies
                .Include(c => c.CompanyCeos)
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == CompanyId &&
                    c.CompanyCeos.Any(cc => cc.CeoUserId == ceoUser.Id));

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No matching company found or you are not authorized."
                });
            }

            // Retrieve all employees of the company
            var employees = await _context.CompanyEmployees
                .Where(e => e.CompanyId == company.CompanyId)
                .Include(e => e.User)  // Join with AspNetUsers
                .ToListAsync();

            if (!employees.Any())
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "No employees found for this company.",
                    Data = Array.Empty<EmployeeDetailsDto>()
                });
            }

            // Map to DTO
            var employeeDtos = _mapper.Map<List<EmployeeDetailsDto>>(employees);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Employees retrieved successfully.",
                Data = employeeDtos
            });
        }

        [HttpPut("ceo/update-company-details")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> UpdateCompanyDetails(
            [FromBody] UpdateCompanyDetailsDto dto,
            [FromQuery] int companyId
        )
        {
            // Extract username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new { StatusCode = 401, Message = "User identity cannot be determined." });
            }

            // Find the user by username
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound(new { StatusCode = 404, Message = $"User with username '{username}' was not found." });
            }

            // Find the target company
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == companyId &&
                    c.CompanyCeos.Any(cc => cc.CeoUserId == user.Id));

            if (company == null)
            {
                return NotFound(new { StatusCode = 404, Message = "No company found matching the criteria or you are not authorized." });
            }

            // Map DTO to existing entity
            _mapper.Map(dto, company, opts => opts.Items["Condition"] = (Func<object, bool>)(srcMember =>
                srcMember != null && (srcMember is not string || !string.IsNullOrWhiteSpace(srcMember.ToString()))
            ));

            // Handle Address and AddressType updates
            var addressType = await _context.AddressTypes
                .FirstOrDefaultAsync(at => at.AddressTypeName == dto.AddressTypeName);

            if (addressType == null)
            {
                addressType = new AddressType
                {
                    AddressTypeName = dto.AddressTypeName,
                    Description = dto.Description
                };
                _context.AddressTypes.Add(addressType);
            }

            var address = await _context.Addresses
                .FirstOrDefaultAsync(a =>
                    a.Street == dto.Street &&
                    a.HouseNumber == dto.HouseNumber &&
                    a.PostalCode == dto.PostalCode &&
                    a.City == dto.City &&
                    a.State == dto.State &&
                    a.Country == dto.Country);

            if (address == null)
            {
                address = new Address
                {
                    Street = dto.Street,
                    HouseNumber = dto.HouseNumber,
                    PostalCode = dto.PostalCode,
                    City = dto.City,
                    State = dto.State,
                    Country = dto.Country
                };
                _context.Addresses.Add(address);
            }

            var companyAddress = company.CompanyAddresses.FirstOrDefault();
            if (companyAddress != null)
            {
                companyAddress.Address = address;
                companyAddress.AddressType = addressType;
                companyAddress.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                company.CompanyAddresses.Add(new CompanyAddress
                {
                    Address = address,
                    AddressType = addressType,
                    IsDefault = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            var updatedFields = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(dto.Name)) updatedFields["Name"] = dto.Name;
            if (!string.IsNullOrWhiteSpace(dto.Industry)) updatedFields["Industry"] = dto.Industry;
            if (!string.IsNullOrWhiteSpace(dto.Description)) updatedFields["Description"] = dto.Description;
            if (!string.IsNullOrWhiteSpace(dto.RegistrationNumber)) updatedFields["RegistrationNumber"] = dto.RegistrationNumber;
            if (!string.IsNullOrWhiteSpace(dto.TaxId)) updatedFields["TaxId"] = dto.TaxId;
            if (!string.IsNullOrWhiteSpace(dto.Website)) updatedFields["Website"] = dto.Website;
            if (!string.IsNullOrWhiteSpace(dto.ContactEmail)) updatedFields["ContactEmail"] = dto.ContactEmail;
            if (!string.IsNullOrWhiteSpace(dto.ContactPhone)) updatedFields["ContactPhone"] = dto.ContactPhone;
            if (!string.IsNullOrWhiteSpace(dto.Street)) updatedFields["Street"] = dto.Street;
            if (!string.IsNullOrWhiteSpace(dto.HouseNumber)) updatedFields["HouseNumber"] = dto.HouseNumber;
            if (!string.IsNullOrWhiteSpace(dto.PostalCode)) updatedFields["PostalCode"] = dto.PostalCode;
            if (!string.IsNullOrWhiteSpace(dto.City)) updatedFields["City"] = dto.City;
            if (!string.IsNullOrWhiteSpace(dto.State)) updatedFields["State"] = dto.State;
            if (!string.IsNullOrWhiteSpace(dto.Country)) updatedFields["Country"] = dto.Country;

            if (dto.FoundedDate.HasValue)
            {
                updatedFields["FoundedDate"] = dto.FoundedDate.Value;
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "Company details and address updated successfully.",
                UpdatedFields = updatedFields
            });
        }

        [HttpPost("ceo/register-company")]
        [Authorize(Roles = "NormalUser, CEO")]
        public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyReqDto dto)
        {
            // Extract username from JWT token
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new { StatusCode = 401, Message = "User identity cannot be determined." });
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Industry))
            {
                return BadRequest(new { StatusCode = 400, Message = "Name and Industry are required fields." });
            }

            // Check if a company with the same registration number already exists
            if (await _context.Companies.AnyAsync(c => c.RegistrationNumber == dto.RegistrationNumber))
            {
                return Conflict(new { StatusCode = 409, Message = "A company with the same registration number already exists." });
            }

            // Check if a company with the same tax ID already exists
            if (await _context.Companies.AnyAsync(c => c.TaxId == dto.TaxId))
            {
                return Conflict(new { StatusCode = 409, Message = "A company with the same tax ID already exists." });
            }

            // Use AutoMapper to map DTO to the Company entity
            var company = _mapper.Map<Company>(dto);

            // Set additional properties if needed
            company.FoundedDate ??= DateOnly.FromDateTime(DateTime.UtcNow);

            // Save the company to the database
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            // Assign "CEO" role to the user
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && !await _userManager.IsInRoleAsync(user, "CEO"))
            {
                await _userManager.AddToRoleAsync(user, "CEO");
            }

            // Add CEO information
            var ceo = new CompanyCeo
            {
                CompanyId = company.CompanyId,
                CeoUserId = user.Id, // Extracted from the UserManager
                StartDate = dto.StartDate
            };

            _context.CompanyCeos.Add(ceo);
            await _context.SaveChangesAsync();

            var ceoDto = _mapper.Map<CompanyCeoDto>(ceo);

            return CreatedAtAction(nameof(RegisterCompany), new { id = company.CompanyId }, new
            {
                StatusCode = 201,
                Message = "Company successfully created, CEO role assigned, and CEO information saved.",
                Data = new
                {
                    company.CompanyId,
                    company.Name,
                    company.Industry,
                    company.Description,
                    company.RegistrationNumber,
                    company.TaxId,
                    company.Website,
                    company.ContactEmail,
                    company.ContactPhone,
                    company.FoundedDate,
                    CompanyCeos = ceoDto
                }
            });
        }

        [HttpPost("ceo/add-employee")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> AddEmployeeToCompany(
            [FromBody] AddEmployeeDto dto,
            [FromQuery] int companyId)
        {
            // Extract CEO username from JWT
            var ceoUsername = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(ceoUsername))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the CEO user
            var ceoUser = await _userManager.FindByNameAsync(ceoUsername);
            if (ceoUser == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "CEO not found."
                });
            }

            // Find the target company by parameters
            var company = await _context.Companies
                .Include(c => c.CompanyCeos)
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == companyId &&
                    c.CompanyCeos.Any(cc => cc.CeoUserId == ceoUser.Id));

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No matching company found or you are not authorized."
                });
            }

            // Find the employee user by username
            var employeeUser = await _userManager.FindByNameAsync(dto.EmployeeUsername);
            if (employeeUser == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"User '{dto.EmployeeUsername}' not found."
                });
            }

            // Check if employee already exists in the company
            bool employeeExists = await _context.CompanyEmployees
                .AnyAsync(e => e.CompanyId == company.CompanyId && e.UserId == employeeUser.Id);

            if (employeeExists)
            {
                return Conflict(new
                {
                    StatusCode = 409,
                    Message = "Employee already assigned to this company."
                });
            }

            // Map DTO to CompanyEmployee entity
            var companyEmployee = _mapper.Map<CompanyEmployee>(dto);
            companyEmployee.UserId = employeeUser.Id;
            companyEmployee.CompanyId = company.CompanyId;

            _context.CompanyEmployees.Add(companyEmployee);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Employee successfully added to the company.",
                Data = new
                {
                    company.Name,
                    dto.Position,
                    dto.StartDate
                }
            });
        }

        [HttpDelete("ceo/delete-all-my-companies")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> DeleteAllMyCompanies()
        {
            // Extract username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the user by username
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"User with username '{username}' was not found."
                });
            }

            // Fetch companies owned by the user
            var companies = await _context.Companies
                .Include(c => c.CompanyCeos) // Include CompanyCeo to filter by user
                .Where(c => c.CompanyCeos.Any(cc => cc.CeoUserId == user.Id))
                .ToListAsync();

            if (!companies.Any())
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "You do not own any companies.",
                    Success = false
                });
            }

            // Remove only the CompanyCeo links related to the user's companies
            var companyCeos = companies.SelectMany(c => c.CompanyCeos.Where(cc => cc.CeoUserId == user.Id)).ToList();
            _context.CompanyCeos.RemoveRange(companyCeos);

            // Remove the companies themselves
            _context.Companies.RemoveRange(companies);

            // Save changes
            await _context.SaveChangesAsync();

            // Remove CEO role if no companies are left associated with the user
            var remainingCompanies = await _context.CompanyCeos.AnyAsync(cc => cc.CeoUserId == user.Id);
            if (!remainingCompanies && await _userManager.IsInRoleAsync(user, "CEO"))
            {
                await _userManager.RemoveFromRoleAsync(user, "CEO");
                await _userManager.UpdateSecurityStampAsync(user);  // Ensure the security stamp is updated
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = $"{companies.Count} companies have been deleted successfully.",
                Success = true
            });
        }

        [HttpDelete("ceo/delete-company")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> DeleteCompanyById([FromQuery] int companyId)
        {
            // Extract CEO username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new { StatusCode = 401, Message = "User identity cannot be determined." });
            }

            // Find the company and validate CEO association
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                .Include(c => c.CompanyCeos)
                .Include(c => c.CompanyEmployees)
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == companyId &&
                    c.CompanyCeos.Any(cc => cc.CeoUser.UserName == username)
                );

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Company not found or you are not authorized to delete this company."
                });
            }

            // Delete associated addresses
            var companyAddresses = company.CompanyAddresses.ToList();
            if (companyAddresses.Any())
            {
                _context.CompanyAddresses.RemoveRange(companyAddresses);
            }

            // Delete associated employees
            var companyEmployees = company.CompanyEmployees.ToList();
            if (companyEmployees.Any())
            {
                _context.CompanyEmployees.RemoveRange(companyEmployees);
            }

            // Delete associated CEO entries
            var ceoEntries = company.CompanyCeos.ToList();
            if (ceoEntries.Any())
            {
                _context.CompanyCeos.RemoveRange(ceoEntries);
            }

            // Delete the company
            _context.Companies.Remove(company);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Company and all associated records deleted successfully."
                });
            }
            catch (DbUpdateException ex)
            {
                // Handle foreign key constraint errors if any
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Failed to delete the company due to linked data. Please try again or contact support.",
                    Error = ex.InnerException?.Message
                });
            }
        }

        [HttpDelete("ceo/delete-employee")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> DeleteEmployee(
    [FromQuery] int companyId,
    [FromQuery] string employeeUsername)
        {
            // Extract CEO username from JWT
            var ceoUsername = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(ceoUsername))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the CEO user
            var ceoUser = await _userManager.FindByNameAsync(ceoUsername);
            if (ceoUser == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "CEO not found."
                });
            }

            // Check if the company belongs to the CEO
            var company = await _context.Companies
                .Include(c => c.CompanyCeos)
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == companyId &&
                    c.CompanyCeos.Any(cc => cc.CeoUserId == ceoUser.Id));

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No company found matching the criteria or you are not authorized."
                });
            }

            // Find the employee by username
            var employee = await _userManager.FindByNameAsync(employeeUsername);
            if (employee == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"Employee with username '{employeeUsername}' not found."
                });
            }

            // Check if the employee is part of the company
            var companyEmployee = await _context.CompanyEmployees
                .FirstOrDefaultAsync(e => e.CompanyId == company.CompanyId && e.UserId == employee.Id);

            if (companyEmployee == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"Employee '{employeeUsername}' is not part of this company."
                });
            }

            // Remove the employee from the company
            _context.CompanyEmployees.Remove(companyEmployee);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = $"Employee '{employeeUsername}' has been successfully removed from {company.Name}."
            });
        }
    }
}