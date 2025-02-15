using AutoMapper;
using CoworkingSpaceAPI.Dtos.Desk.Request;
using CoworkingSpaceAPI.Dtos.Desk.Response;
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
    public class DeskController : ControllerBase
    {
        private readonly CoworkingSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DeskController(CoworkingSpaceDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/Desk/ceo/get-all-desks-of-company
        [HttpGet("ceo/get-all-desks-of-company")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> GetAllDesksOfCompany(
            [FromQuery] int companyId)
        {
            // Extract CEO username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the CEO user
            var ceoUser = await _userManager.FindByNameAsync(username);
            if (ceoUser == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "CEO not found."
                });
            }

            // Validate that the company exists and the CEO is associated with it
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                    .ThenInclude(ca => ca.Rooms)
                        .ThenInclude(r => r.Desks)
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

            // Retrieve all desks for the specified company
            var desks = company.CompanyAddresses
                .SelectMany(ca => ca.Rooms)
                .SelectMany(r => r.Desks)
                .ToList();

            if (!desks.Any())
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "No desks found for this company.",
                    Data = Array.Empty<DeskDetailsWithRoomDto>()
                });
            }

            // Map desks to DTO
            var deskDtos = _mapper.Map<List<DeskDetailsWithRoomDto>>(desks);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Desks retrieved successfully.",
                Data = deskDtos
            });
        }

        // GET: api/Desk/ceo/get-all-desks
        [HttpGet("ceo/get-desks-in-a-room")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> GetAllDesksInRoom(
            [FromQuery] int companyId,
            [FromQuery] int roomId)
        {
            // Extract CEO username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the CEO user
            var ceoUser = await _userManager.FindByNameAsync(username);
            if (ceoUser == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "CEO not found."
                });
            }

            // Validate that the company exists and the CEO is associated with it
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                    .ThenInclude(ca => ca.Rooms)
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

            // Validate the room exists under the company
            var room = company.CompanyAddresses
                .SelectMany(ca => ca.Rooms)
                .FirstOrDefault(r => r.RoomId == roomId);

            if (room == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Room not found for this company."
                });
            }

            // Retrieve all desks for the specified room
            var desks = await _context.Desks
                .Where(d => d.RoomId == roomId)
                .ToListAsync();

            if (!desks.Any())
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "No desks found for this room.",
                    Data = Array.Empty<DeskDetailsDto>()
                });
            }

            // Map desks to DTO
            var deskDtos = _mapper.Map<List<DeskDetailsDto>>(desks);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Desks retrieved successfully.",
                Data = deskDtos
            });
        }

        // POST: api/Desk/ceo/add-desk
        [HttpPost("ceo/add-desk")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> AddDesk(
            [FromBody] AddDeskDto dto,
            [FromQuery] int companyId,
            [FromQuery] int roomId)
        {
            // Extract CEO username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the CEO user
            var ceoUser = await _userManager.FindByNameAsync(username);
            if (ceoUser == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "CEO not found."
                });
            }

            // Validate that the company exists and the CEO is associated with it
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
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

            // Check if the room exists under this company
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r =>
                    r.RoomId == roomId &&
                    company.CompanyAddresses
                        .Select(ca => ca.CompanyAddressId)
                        .Contains((int)r.CompanyAddressId));

            if (room == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Room not found for this company."
                });
            }

            // Map DTO to Desk entity
            var desk = _mapper.Map<Desk>(dto);
            desk.RoomId = roomId;
            desk.CreatedAt = DateTime.UtcNow;

            // Save desk to database
            _context.Desks.Add(desk);
            await _context.SaveChangesAsync();

            // Prepare response
            return Ok(new
            {
                StatusCode = 201,
                Message = "Desk successfully added to the room.",
                Data = new
                {
                    desk.DeskId,
                    desk.DeskName,
                    desk.Price,
                    desk.Currency,
                    desk.IsAvailable,
                    desk.CreatedAt,
                    desk.UpdatedAt,
                    desk.RoomId
                }
            });
        }

        // PUT: api/Desk/ceo/update-desk
        [HttpPut("ceo/update-desk")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> UpdateDesk(
            [FromBody] UpdateDeskDto dto,
            [FromQuery] int companyId,
            [FromQuery] int roomId,
            [FromQuery] int deskId)
        {
            // Extract CEO username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the CEO user
            var ceoUser = await _userManager.FindByNameAsync(username);
            if (ceoUser == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "CEO not found."
                });
            }

            // Validate company ownership
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                    .ThenInclude(ca => ca.Rooms)
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

            // Retrieve the desk by deskId and roomId
            var roomIds = company.CompanyAddresses
                .SelectMany(ca => ca.Rooms.Select(r => r.RoomId))
                .ToList();

            var desk = await _context.Desks
                .FirstOrDefaultAsync(d =>
                    d.DeskId == deskId &&
                    d.RoomId == roomId &&
                    roomIds.Contains(roomId));

            if (desk == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Desk not found for the specified room or company."
                });
            }

            // Map updates from DTO to Desk
            _mapper.Map(dto, desk);

            // If NewRoomId is provided, update the RoomId
            if (dto.NewRoomId.HasValue)
            {
                var newRoom = company.CompanyAddresses
                    .SelectMany(ca => ca.Rooms)
                    .FirstOrDefault(r => r.RoomId == dto.NewRoomId.Value);

                if (newRoom == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Invalid room specified. Desk cannot be moved."
                    });
                }

                desk.RoomId = newRoom.RoomId;
            }

            // Update timestamp
            desk.UpdatedAt = DateTime.UtcNow;

            // Save changes
            await _context.SaveChangesAsync();

            // Return updated desk details
            var updatedDesk = _mapper.Map<DeskDetailsWithRoomDto>(desk);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Desk updated successfully.",
                Data = updatedDesk
            });
        }

        // DELETE: api/Desk/ceo/delete-all-desks-in-room
        [HttpDelete("ceo/delete-all-desks-in-room")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> DeleteAllDesksInRoom([FromQuery] DeleteAllDesksInRoomRequestDto dto)
        {
            // Extract CEO username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the CEO user
            var ceoUser = await _userManager.FindByNameAsync(username);
            if (ceoUser == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "CEO not found."
                });
            }

            // Validate that the company exists and the CEO is associated with it
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                    .ThenInclude(ca => ca.Rooms)
                        .ThenInclude(r => r.Desks)
                .Include(c => c.CompanyCeos)
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == dto.CompanyId &&
                    c.CompanyCeos.Any(cc => cc.CeoUserId == ceoUser.Id));

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No company found matching the criteria or you are not authorized."
                });
            }

            // Find the room by roomId
            var room = company.CompanyAddresses
                .SelectMany(ca => ca.Rooms)
                .FirstOrDefault(r => r.RoomId == dto.RoomId);

            if (room == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Room not found for the specified company."
                });
            }

            // Find all desks associated with the room
            var desksToDelete = room.Desks.ToList();
            if (!desksToDelete.Any())
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "No desks found in the specified room."
                });
            }

            // Map desks to DTOs before deletion
            var desksDto = _mapper.Map<List<DeskDetailsWithRoomDto>>(desksToDelete);

            // Delete all desks
            _context.Desks.RemoveRange(desksToDelete);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "All desks in the room deleted successfully.",
                Data = desksDto
            });
        }

        // DELETE: api/Desk/ceo/delete-all-desks-in-company
        [HttpDelete("ceo/delete-all-desks-in-company")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> DeleteAllDesksInCompany([FromQuery] DeleteAllDesksInCompanyRequestDto dto)
        {
            // Extract CEO username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the CEO user
            var ceoUser = await _userManager.FindByNameAsync(username);
            if (ceoUser == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "CEO not found."
                });
            }

            // Validate that the company exists and the CEO is associated with it
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                    .ThenInclude(ca => ca.Rooms)
                        .ThenInclude(r => r.Desks)
                .Include(c => c.CompanyCeos)
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == dto.CompanyId &&
                    c.CompanyCeos.Any(cc => cc.CeoUserId == ceoUser.Id));

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No company found matching the criteria or you are not authorized."
                });
            }

            // Collect all desks in the company's rooms
            var desksToDelete = company.CompanyAddresses
                .SelectMany(ca => ca.Rooms)
                .SelectMany(r => r.Desks)
                .ToList();

            if (!desksToDelete.Any())
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "No desks found in the specified company."
                });
            }

            // Map desks to DTOs before deletion
            var desksDto = _mapper.Map<List<DeskDetailsWithRoomDto>>(desksToDelete);

            // Delete all desks
            _context.Desks.RemoveRange(desksToDelete);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "All desks in the company deleted successfully.",
                Data = desksDto
            });
        }

        // DELETE: api/Desk/ceo/delete-desk
        [HttpDelete("ceo/delete-desk")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> DeleteDesk([FromQuery] DeleteDeskRequestDto dto)
        {
            // Extract CEO username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            // Find the CEO user
            var ceoUser = await _userManager.FindByNameAsync(username);
            if (ceoUser == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "CEO not found."
                });
            }

            // Validate that the company exists and the CEO is associated with it
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                    .ThenInclude(ca => ca.Rooms)
                        .ThenInclude(r => r.Desks)
                .Include(c => c.CompanyCeos)
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == dto.CompanyId &&
                    c.CompanyCeos.Any(cc => cc.CeoUserId == ceoUser.Id));

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No company found matching the criteria or you are not authorized."
                });
            }

            // Find the desk by deskId
            var desk = company.CompanyAddresses
                .SelectMany(ca => ca.Rooms)
                .SelectMany(r => r.Desks)
                .FirstOrDefault(d => d.DeskId == dto.DeskId);

            if (desk == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Desk not found for the specified company."
                });
            }

            // Map desk to DTO before deletion
            var deskDto = _mapper.Map<DeskDetailsWithRoomDto>(desk);

            // Delete the desk
            _context.Desks.Remove(desk);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Desk deleted successfully.",
                Data = deskDto
            });
        }
    }
}