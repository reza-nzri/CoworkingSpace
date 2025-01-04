using AutoMapper;
using CoworkingSpaceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}