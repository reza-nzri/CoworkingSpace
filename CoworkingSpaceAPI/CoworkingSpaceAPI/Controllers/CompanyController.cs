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

        // GET: api/Company
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        // GET: api/Company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
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

            foreach (var company in companies)
            {
                var companyDto = companyDetailsDtos.FirstOrDefault(c => c.Name == company.Name);

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

            // Map the company to a DTO
            var companyDetailsDtoList = _mapper.Map<List<CompanyDetailsDto>>(companies);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Company details retrieved successfully.",
                Success = true,
                Data = companyDetailsDtoList
            });
        }

        [HttpPut("ceo/update-company-details")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> UpdateCompanyDetails([FromBody] UpdateCompanyDetailsDto dto)
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

            if (string.IsNullOrWhiteSpace(dto.FoundedDate?.ToString()))
            {
                dto.FoundedDate = null; // Set null if empty
            }

            // Find the company associated with the user
            var company = await _context.Companies
                .Include(c => c.CompanyCeos)
                .FirstOrDefaultAsync(c => c.CompanyCeos.Any(cc => cc.CeoUserId == user.Id));

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No company associated with the user."
                });
            }

            // Track the changes
            var changes = new List<string>();

            // Update only the fields provided in the DTO
            if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != company.Name)
            {
                company.Name = dto.Name;
                changes.Add($"Name updated to '{dto.Name}'.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Industry) && dto.Industry != company.Industry)
            {
                company.Industry = dto.Industry;
                changes.Add($"Industry updated to '{dto.Industry}'.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Description) && dto.Description != company.Description)
            {
                company.Description = dto.Description;
                changes.Add($"Description updated to '{dto.Description}'.");
            }

            if (!string.IsNullOrWhiteSpace(dto.RegistrationNumber) && dto.RegistrationNumber != company.RegistrationNumber)
            {
                company.RegistrationNumber = dto.RegistrationNumber;
                changes.Add($"RegistrationNumber updated to '{dto.RegistrationNumber}'.");
            }

            if (!string.IsNullOrWhiteSpace(dto.TaxId) && dto.TaxId != company.TaxId)
            {
                company.TaxId = dto.TaxId;
                changes.Add($"TaxId updated to '{dto.TaxId}'.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Website) && dto.Website != company.Website)
            {
                company.Website = dto.Website;
                changes.Add($"Website updated to '{dto.Website}'.");
            }

            if (!string.IsNullOrWhiteSpace(dto.ContactEmail) && dto.ContactEmail != company.ContactEmail)
            {
                company.ContactEmail = dto.ContactEmail;
                changes.Add($"ContactEmail updated to '{dto.ContactEmail}'.");
            }

            if (!string.IsNullOrWhiteSpace(dto.ContactPhone) && dto.ContactPhone != company.ContactPhone)
            {
                company.ContactPhone = dto.ContactPhone;
                changes.Add($"ContactPhone updated to '{dto.ContactPhone}'.");
            }

            if (dto.FoundedDate.HasValue && dto.FoundedDate.Value != company.FoundedDate)
            {
                company.FoundedDate = dto.FoundedDate.Value;
                changes.Add($"FoundedDate updated to '{dto.FoundedDate.Value}'.");
            }

            // Save changes if any
            if (changes.Count > 0)
            {
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Company details updated successfully.",
                    Changes = changes
                });
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "No changes were made.",
                Changes = changes
            });
        }

        // PUT: api/Company/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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

        // POST: api/Company
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
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

        // DELETE: api/Company/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}