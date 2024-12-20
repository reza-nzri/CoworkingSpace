using AutoMapper;
using CoworkingSpaceAPI.Dtos.Address.Request;
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
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly CoworkingSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddressController(CoworkingSpaceDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/Address
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            return await _context.Addresses.ToListAsync();
        }

        // GET: api/Address/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            var address = await _context.Addresses.FindAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        // GET: api/User/get-all-address-types
        [HttpGet("get-all-address-types")]
        public async Task<ActionResult<IEnumerable<AddressType>>> GetAllAddressTypes()
        {
            var addressTypes = await _context.AddressTypes
                .Select(at => new
                {
                    at.AddressTypeId,
                    at.AddressTypeName,
                    at.Description
                })
                .ToListAsync();

            if (addressTypes == null || !addressTypes.Any())
            {
                return NotFound(new { StatusCode = 404, Message = "No address types found." });
            }

            return Ok(new { StatusCode = 200, Message = "Address types retrieved successfully.", Data = addressTypes });
        }

        // PUT: api/Address/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, Address address)
        {
            if (id != address.AddressId)
            {
                return BadRequest();
            }

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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

        // POST: api/Address
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post-address")]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAddress", new { id = address.AddressId }, address);
        }

        [HttpPost("ceo/add-address")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> AddAddress([FromBody] AddAddressDto dto)
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

            // Validate the company associated with the CEO
            var company = await _context.Companies
                .Include(c => c.CompanyCeos)
                .FirstOrDefaultAsync(c => c.CompanyCeos.Any(cc => cc.CeoUserId == user.Id) && c.Name == dto.CompanyName);

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"No company associated with '{dto.CompanyName}' was found for the user."
                });
            }

            // Map DTO to Address entity
            var address = _mapper.Map<Address>(dto);

            // Add AddressType if it doesn't exist
            var addressType = await _context.AddressTypes.FirstOrDefaultAsync(at => at.AddressTypeName == dto.AddressTypeName);
            if (addressType == null)
            {
                addressType = new AddressType
                {
                    AddressTypeName = dto.AddressTypeName,
                    Description = dto.AddressTypeDescription
                };
                _context.AddressTypes.Add(addressType);
                await _context.SaveChangesAsync();
            }

            // Create and link the address to the company
            var companyAddress = new CompanyAddress
            {
                CompanyId = company.CompanyId,
                Address = address,
                AddressTypeId = addressType.AddressTypeId,
                IsDefault = dto.IsDefault ?? false,
                CreatedAt = DateTime.UtcNow,
            };

            _context.CompanyAddresses.Add(companyAddress);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 201,
                Message = "Address added successfully.",
                Data = new
                {
                    CompanyAddressId = companyAddress.CompanyAddressId,
                    CompanyName = company.Name,
                    AddressType = addressType.AddressTypeName,
                    AddressDetails = new
                    {
                        address.Street,
                        address.HouseNumber,
                        address.PostalCode,
                        address.City,
                        address.State,
                        address.Country
                    }
                }
            });
        }

        [HttpDelete("delete-address")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.AddressId == id);
        }
    }
}