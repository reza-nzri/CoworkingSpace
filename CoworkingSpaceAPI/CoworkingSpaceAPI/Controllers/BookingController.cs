using AutoMapper;
using CoworkingSpaceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly CoworkingSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BookingController(CoworkingSpaceDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }
    }
}