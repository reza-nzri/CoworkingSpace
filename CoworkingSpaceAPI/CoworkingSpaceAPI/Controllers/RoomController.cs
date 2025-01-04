using AutoMapper;
using CoworkingSpaceAPI.Dtos.Room;
using CoworkingSpaceAPI.Dtos.Room.Request;
using CoworkingSpaceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RoomDetailsDto = CoworkingSpaceAPI.Dtos.Room.Request.RoomDetailsDto;

namespace CoworkingSpaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly CoworkingSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoomController(CoworkingSpaceDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/Room/ceo/get-all-rooms
        [HttpGet("ceo/get-all-rooms")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> GetAllRooms([FromQuery] int companyId)
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

            // Fetch the company by provided details
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
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

            // Retrieve all rooms associated with the company's address
            var companyAddressIds = company.CompanyAddresses
                .Select(ca => ca.CompanyAddressId)
                .ToList();

            var rooms = await _context.Rooms
                .Where(r => r.CompanyAddressId != null && companyAddressIds.Contains((int)r.CompanyAddressId))
                .ToListAsync();

            if (rooms == null || !rooms.Any())
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "No rooms found for this company.",
                    Data = new List<object>()
                });
            }

            var roomDetails = rooms.Select(r => new Dtos.Room.Request.RoomDetailsDto
            {
                RoomId = r.RoomId,
                RoomName = r.RoomName,
                RoomType = r.RoomType,
                Price = r.Price ?? 0,  // Default to 0 if null
                Currency = r.Currency ?? "N/A",
                IsActive = r.IsActive,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Rooms retrieved successfully.",
                Data = roomDetails
            });
        }

        [HttpPut("ceo/update-room")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> UpdateRoom(
            [FromBody] UpdateRoomDto dto,
            [FromQuery] int companyId,
            [FromQuery] int RoomId)
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

            // Fetch the company by provided details
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
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

            // Retrieve the room by RoomId and CompanyAddressId
            var companyAddressIds = company.CompanyAddresses.Select(ca => ca.CompanyAddressId).ToList();

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r =>
                    r.RoomId == RoomId &&
                    r.CompanyAddressId != null &&
                    companyAddressIds.Contains((int)r.CompanyAddressId));

            if (room == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Room not found for the specified company."
                });
            }

            // Update allowed properties if provided
            if (!string.IsNullOrWhiteSpace(dto.RoomName)) room.RoomName = dto.RoomName;
            if (!string.IsNullOrWhiteSpace(dto.RoomType)) room.RoomType = dto.RoomType;
            if (dto.Price.HasValue) room.Price = dto.Price;
            if (!string.IsNullOrWhiteSpace(dto.Currency)) room.Currency = dto.Currency;
            if (dto.IsActive.HasValue) room.IsActive = dto.IsActive.Value;

            // Update timestamp
            room.UpdatedAt = DateTime.UtcNow;

            // Save changes
            await _context.SaveChangesAsync();

            // Map to DTO for response
            var updatedRoom = new Dtos.Room.Request.RoomDetailsDto
            {
                RoomId = room.RoomId,
                RoomName = room.RoomName,
                RoomType = room.RoomType,
                Price = (decimal)room.Price,
                Currency = room.Currency,
                IsActive = room.IsActive,
                CreatedAt = room.CreatedAt,
                UpdatedAt = room.UpdatedAt
            };

            return Ok(new
            {
                StatusCode = 200,
                Message = "Room updated successfully.",
                Data = updatedRoom
            });
        }

        // POST: api/Company/ceo/add-room
        [HttpPost("ceo/add-room")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> AddRoom(
            [FromBody] AddRoomDto dto,
            [FromQuery] int companyId)
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

            // Validate company associated with the CEO
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                .Include(c => c.CompanyCeos)
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == companyId &&
                    c.CompanyCeos.Any(cc => cc.CeoUserId == user.Id));

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"No company found with the specified criteria or you are not authorized."
                });
            }

            // Check for company address
            var companyAddress = company.CompanyAddresses.FirstOrDefault();
            if (companyAddress == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please update the company address before adding a room."
                });
            }

            // Map DTO to Room entity
            var room = _mapper.Map<Room>(dto);
            room.CompanyAddressId = companyAddress.CompanyAddressId;
            room.IsActive = dto.IsActive ?? true;  // Default to true if not provided
            room.CreatedAt = DateTime.UtcNow;

            // Save the room to database
            var roomEntity = _mapper.Map<CoworkingSpaceAPI.Models.Room>(room);
            _context.Rooms.Add(roomEntity);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 201,
                Message = "Room successfully added to the company.",
                Data = new
                {
                    room.RoomId,
                    room.RoomName,
                    room.RoomType,
                    room.Price,
                    room.Currency,
                    room.IsActive,
                    room.CreatedAt,
                    room.UpdatedAt,
                    room.CompanyAddressId
                }
            });
        }

        [HttpDelete("ceo/delete-all-rooms-in-company")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> DeleteAllRoomsInCompany([FromQuery] int companyId)
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

            // Fetch the company by ID and check CEO authorization
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                .ThenInclude(ca => ca.Rooms)
                .ThenInclude(r => r.Desks)
                .ThenInclude(d => d.Bookings)
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

            // Collect all rooms and related entities
            var roomsToDelete = company.CompanyAddresses
                .SelectMany(ca => ca.Rooms)
                .ToList();

            if (!roomsToDelete.Any())
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "No rooms found for this company to delete.",
                    Data = Array.Empty<object>()
                });
            }

            // Collect all desks and bookings
            var desksToDelete = roomsToDelete
                .SelectMany(r => r.Desks)
                .ToList();

            var bookingsToDelete = desksToDelete
                .SelectMany(d => d.Bookings)
                .ToList();

            // Delete bookings
            if (bookingsToDelete.Any())
            {
                _context.Bookings.RemoveRange(bookingsToDelete);
            }

            // Delete desks
            if (desksToDelete.Any())
            {
                _context.Desks.RemoveRange(desksToDelete);
            }

            // Delete rooms
            _context.Rooms.RemoveRange(roomsToDelete);

            // Save changes
            await _context.SaveChangesAsync();

            List<RoomDetailsDto> deletedRoomDtos = new List<RoomDetailsDto>();

            try
            {
                // Attempt to map to DTO for response
                deletedRoomDtos = _mapper.Map<List<RoomDetailsDto>>(roomsToDelete);
            }
            catch (AutoMapperMappingException ex)
            {
                // Log the exception or handle it as necessary (optional)
                Console.WriteLine($"Mapping error: {ex.Message}");
                // Continue with successful deletion message even if mapping fails
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "All rooms and associated entities deleted successfully.",
                Data = deletedRoomDtos
            });
        }

        [HttpDelete("ceo/delete-room")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> DeleteRoom(
            [FromQuery] int companyId,
            [FromQuery] int RoomId)
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

            // Fetch the company by provided details
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
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

            // Retrieve the room by RoomId and CompanyAddressId
            var companyAddressIds = company.CompanyAddresses.Select(ca => ca.CompanyAddressId).ToList();

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r =>
                    r.RoomId == RoomId &&
                    r.CompanyAddressId != null &&
                    companyAddressIds.Contains((int)r.CompanyAddressId));

            if (room == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Room not found for the specified company."
                });
            }

            // Map to DTO for response before deletion
            var roomDto = new RoomDetailsDto
            {
                RoomId = room.RoomId,
                RoomName = room.RoomName,
                RoomType = room.RoomType,
                Price = (decimal)room.Price,
                Currency = room.Currency,
                IsActive = room.IsActive,
                CreatedAt = room.CreatedAt,
                UpdatedAt = room.UpdatedAt
            };

            // Delete room
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Room deleted successfully.",
                Data = roomDto
            });
        }
    }
}