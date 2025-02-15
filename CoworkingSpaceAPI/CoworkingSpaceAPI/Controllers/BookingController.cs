using AutoMapper;
using CoworkingSpaceAPI.Dtos.Booking.Request;
using CoworkingSpaceAPI.Dtos.Booking.Response;
using CoworkingSpaceAPI.Dtos.Booking.Response.CoworkingSpaceAPI.Dtos.Booking;
using CoworkingSpaceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        [HttpGet("get-my-bookings")]
        [Authorize]
        public async Task<IActionResult> GetMyBookings()
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found."
                });
            }

            // Fetch bookings with related data (Room, Desk, and Company details)
            var bookings = await _context.Bookings
                .Where(b => b.UserId == user.Id)
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .ThenInclude(d => d.Room)
                .ThenInclude(r => r.CompanyAddress)
                .ThenInclude(ca => ca.Company)
                .ToListAsync();

            if (bookings == null || !bookings.Any())
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "No bookings found for this user.",
                    Data = new List<object>()
                });
            }

            // Map to DTO for response
            var bookingDtos = bookings.Select(b => new BookingDetailsDto
            {
                BookingId = b.BookingId,
                RoomId = b.RoomId,
                RoomName = b.Room?.RoomName,
                RoomType = b.Room?.RoomType,
                DeskId = b.DeskId,
                DeskName = b.Desk?.DeskName,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                TotalCost = b.TotalCost,
                IsCancelled = b.IsCancelled,
                CancellationReason = b.CancellationReason,
                IsCheckedIn = b.IsCheckedIn,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                CompanyName = b.Room?.CompanyAddress?.Company?.Name,
                Industry = b.Room?.CompanyAddress?.Company?.Industry,
                Website = b.Room?.CompanyAddress?.Company?.Website
            }).ToList();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Bookings retrieved successfully.",
                Data = bookingDtos
            });
        }

        [HttpGet("get-booking-by-username")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> GetBookingByUsername(
             [FromQuery] string username,
             [FromQuery] DateTime? startTime,
             [FromQuery] DateTime? endTime,
             [FromQuery] bool? isCancelled)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Username is required."
                });
            }

            // Check if the user exists
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found."
                });
            }

            if (startTime.HasValue && endTime.HasValue && startTime >= endTime)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Start time must be earlier than end time."
                });
            }

            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .ThenInclude(r => r.CompanyAddress)
                .ThenInclude(ca => ca.Company)
                .Include(b => b.Desk)
                .Where(b => b.UserId == user.Id)
                .Where(b => !startTime.HasValue || b.StartTime >= startTime)
                .Where(b => !endTime.HasValue || b.EndTime <= endTime)
                .Where(b => !isCancelled.HasValue || b.IsCancelled == isCancelled)
                .ToListAsync();

            if (!bookings.Any())
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No bookings found for the specified user."
                });
            }

            var bookingDtos = _mapper.Map<List<BookingDetailsDto>>(bookings);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Bookings retrieved successfully.",
                Bookings = bookingDtos
            });
        }

        [HttpGet("check-availability-desk")]
        [Authorize]
        public async Task<IActionResult> CheckAvailabilityDesk(
            [FromQuery] int CompanyId,
            [FromQuery] int RoomId,
            [FromQuery] int DeskId,
            [FromQuery] DateTime StartTime,
            [FromQuery] DateTime EndTime)
        {
            // Validate input parameters
            if (CompanyId <= 0 || RoomId <= 0 || DeskId <= 0 || StartTime >= EndTime)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid input. Please check the provided details."
                });
            }

            // Check if the company exists
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                .ThenInclude(ca => ca.Rooms)
                .ThenInclude(r => r.Desks)
                .FirstOrDefaultAsync(c => c.CompanyId == CompanyId);

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Company not found."
                });
            }

            // Check if the room exists within the company
            var room = company.CompanyAddresses
                .SelectMany(ca => ca.Rooms)
                .FirstOrDefault(r => r.RoomId == RoomId);

            if (room == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Room not found for the specified company."
                });
            }

            // Check if the desk exists in the room
            var desk = room.Desks.FirstOrDefault(d => d.DeskId == DeskId);
            if (desk == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Desk not found in the specified room."
                });
            }

            // Check for booking conflicts
            var isBooked = await _context.Bookings.AnyAsync(b =>
                b.DeskId == DeskId &&
                b.StartTime < EndTime &&
                b.EndTime > StartTime &&
                !b.IsCancelled);  // Consider only active bookings

            if (isBooked)
            {
                // Suggest next available time
                var nextAvailableBooking = await _context.Bookings
                    .Where(b => b.DeskId == DeskId && b.StartTime > EndTime)
                    .OrderBy(b => b.StartTime)
                    .FirstOrDefaultAsync();

                var nextAvailableTime = nextAvailableBooking?.StartTime;

                // Suggest alternative desks in the same room
                var alternativeDesks = room.Desks
                    .Where(d => d.DeskId != DeskId && !d.Bookings.Any(b =>
                        b.StartTime < EndTime &&
                        b.EndTime > StartTime &&
                        !b.IsCancelled))
                    .Select(_mapper.Map<CheckAvailabilityDeskDto>)
                    .ToList();

                return Ok(new
                {
                    StatusCode = 400,
                    Message = "Desk is already booked for the specified time.",
                    NextAvailableTime = nextAvailableTime,
                    AlternativeDesks = alternativeDesks
                });
            }

            // Desk is available
            var deskDetails = _mapper.Map<CheckAvailabilityDeskDto>(desk);

            return Ok(new
            {
                StatusCode = 200,
                Message = "The desk is available for the selected time slot.",
                DeskDetails = deskDetails
            });
        }

        [HttpGet("check-availability-room")]
        [Authorize]
        public async Task<IActionResult> CheckAvailabilityRoom(
            [FromQuery] int companyId,
            [FromQuery] int roomId,
            [FromQuery] DateTime startTime,
            [FromQuery] DateTime endTime)
        {
            if (companyId <= 0 || roomId <= 0 || startTime == default || endTime == default)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid input. Ensure all required parameters are provided."
                });
            }

            if (startTime >= endTime)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Start time must be earlier than end time."
                });
            }

            // Check if the room exists within the company
            var room = await _context.Rooms
                .Include(r => r.CompanyAddress)
                .ThenInclude(ca => ca.Company)
                .FirstOrDefaultAsync(r => r.RoomId == roomId && r.CompanyAddress.CompanyId == companyId);

            if (room == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Room or company not found."
                });
            }

            // Check if the room is already booked for the given time
            var isBooked = await _context.Bookings.AnyAsync(b =>
                b.RoomId == roomId &&
                !b.IsCancelled &&
                ((startTime < b.EndTime && endTime > b.StartTime))
            );

            if (isBooked)
            {
                // Find next available time slot
                var nextAvailableTime = await _context.Bookings
                    .Where(b => b.RoomId == roomId && b.EndTime > endTime && b.IsCancelled == false)
                    .OrderBy(b => b.EndTime)
                    .Select(b => b.EndTime)
                    .FirstOrDefaultAsync();

                // Suggest alternative rooms
                var alternativeRooms = await _context.Rooms
                    .Where(r => r.CompanyAddress.CompanyId == companyId && r.IsActive && r.RoomId != roomId)
                    .Select(r => new
                    {
                        r.RoomId,
                        r.RoomName,
                        r.RoomType,
                        r.Price,
                        r.Currency,
                        r.IsActive
                    })
                    .ToListAsync();

                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Room is already booked for the specified time.",
                    NextAvailableTime = nextAvailableTime,
                    AlternativeRooms = alternativeRooms
                });
            }

            // Return successful response if room is available
            return Ok(new
            {
                StatusCode = 200,
                Message = "The room is available for the selected time slot.",
                RoomDetails = new
                {
                    room.RoomId,
                    room.RoomName,
                    room.RoomType,
                    room.Price,
                    room.Currency,
                    room.IsActive
                }
            });
        }

        [HttpGet("monthly-costs")]
        [Authorize]
        public async Task<IActionResult> GetMonthlyBookingCosts(
            [FromQuery] int companyId,
            [FromQuery] int year,
            [FromQuery] int month,
            [FromQuery] string? username = null)
        {
            // Input validation
            if (companyId <= 0 || year <= 0 || month <= 0 || month > 12)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid input. Please provide a valid company ID, year, and month."
                });
            }

            // Validate if the company exists
            var companyExists = await _context.Companies
                .AnyAsync(c => c.CompanyId == companyId);

            if (!companyExists)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Company not found."
                });
            }

            // Validate User by Username (if provided)
            ApplicationUserModel? user = null;
            if (!string.IsNullOrWhiteSpace(username))
            {
                user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == username);

                if (user == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User not found."
                    });
                }
            }

            // Fetch bookings for the company and specified time frame
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .Where(b =>
                    b.Room.CompanyAddress.CompanyId == companyId &&
                    b.StartTime.Year == year &&
                    b.StartTime.Month == month &&
                    (user == null || b.UserId == user.Id))
                .ToListAsync();

            // Check if there are bookings
            if (!bookings.Any())
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No bookings found for the specified month and company."
                });
            }

            // Calculate total cost
            var totalCost = bookings.Sum(b => b.TotalCost);

            // Map bookings to DTOs
            var reportDtos = _mapper.Map<List<MonthlyCostReportDto>>(bookings);

            // Return the monthly cost report
            return Ok(new
            {
                StatusCode = 200,
                Message = "Monthly costs calculated successfully.",
                Year = year,
                Month = month,
                TotalCost = totalCost,
                Currency = "EUR",
                Bookings = reportDtos
            });
        }

        [HttpGet("did-not-check-in")]
        [Authorize(Roles = "Admin,Manager, CEO")]
        public async Task<IActionResult> GetNoShowBookings(
             [FromQuery] string Username,
             [FromQuery] DateTime StartDate,
             [FromQuery] DateTime EndDate,
             [FromQuery] int CompanyId)
        {
            // Validate input parameters
            if (string.IsNullOrWhiteSpace(Username))
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Username is required."
                });
            }

            if (StartDate >= EndDate)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Start date must be earlier than end date."
                });
            }

            if (CompanyId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid CompanyId."
                });
            }

            // Verify user existence
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == Username);

            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found."
                });
            }

            // Retrieve bookings that meet no-show criteria
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .ThenInclude(r => r.CompanyAddress)
                .ThenInclude(ca => ca.Company)
                .Where(b => b.UserId == user.Id &&
                            b.Room.CompanyAddress.CompanyId == CompanyId &&
                            b.StartTime >= StartDate &&
                            b.EndTime <= EndDate &&
                            !b.IsCheckedIn &&
                            !b.IsCancelled)
                .Select(b => new NoShowBookingDto
                {
                    BookingId = b.BookingId,
                    RoomName = b.Room.RoomName,
                    DeskId = b.DeskId,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    IsCancelled = b.IsCancelled,
                    CreatedAt = b.CreatedAt,
                    CancellationReason = b.CancellationReason
                })
                .ToListAsync();

            if (bookings == null || !bookings.Any())
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No no-show bookings found for the specified period and company."
                });
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "No-show bookings retrieved successfully.",
                TotalNoShows = bookings.Count,
                Bookings = bookings
            });
        }

        [HttpGet("ceo/average-duration")]
        [Authorize(Roles = "CEO, Admin")]
        public async Task<IActionResult> GetAverageBookingDuration([FromQuery] BookingDurationRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid input. Please provide valid company and date range."
                });
            }

            if (dto.StartDate >= dto.EndDate)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Start date must be earlier than end date."
                });
            }

            var companyExists = await _context.Companies
                .AnyAsync(c => c.CompanyId == dto.CompanyId);

            if (!companyExists)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Company not found."
                });
            }

            var result = await _context.Bookings
                .Where(b =>
                    b.Room.CompanyAddress.CompanyId == dto.CompanyId &&
                    b.StartTime >= dto.StartDate &&
                    b.EndTime <= dto.EndDate)
                .GroupBy(b => b.Room.CompanyAddress.CompanyId)
                .Select(group => new
                {
                    AverageDurationHours = group.Average(b =>
                        EF.Functions.DateDiffHour(b.StartTime, b.EndTime)),
                    TotalBookings = group.Count()
                })
                .FirstOrDefaultAsync();

            if (result == null || result.TotalBookings == 0)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No bookings found for the specified time period and company."
                });
            }

            var response = new BookingDurationResponseDto
            {
                AverageDurationHours = result.AverageDurationHours,
                TotalBookings = result.TotalBookings,
                CompanyId = dto.CompanyId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            return Ok(new
            {
                StatusCode = 200,
                Message = "Average booking duration calculated successfully.",
                Data = response
            });
        }

        // GET: /admin/get-revenue-breakdown
        [HttpGet("get-revenue-breakdown")]
        [Authorize(Roles = "Admin,SuperAdmin, CEO")]
        public async Task<IActionResult> GetRevenueBreakdown(
            [FromQuery] int companyId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int? roomId = null,
            [FromQuery] bool includeDesks = true)
        {
            if (companyId <= 0 || startDate == default || endDate == default)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid input. CompanyId, StartDate, and EndDate are required."
                });
            }

            if (startDate >= endDate)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Start date must be earlier than end date."
                });
            }

            var companyExists = await _context.Companies
                .AnyAsync(c => c.CompanyId == companyId);

            if (!companyExists)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Company not found."
                });
            }

            var query = _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .Where(b =>
                    b.Room.CompanyAddress.CompanyId == companyId &&
                    b.StartTime >= startDate &&
                    b.EndTime <= endDate);

            if (roomId.HasValue)
            {
                query = query.Where(b => b.RoomId == roomId);
            }

            if (!includeDesks)
            {
                query = query.Where(b => b.DeskId == null);
            }

            var results = await query
                .GroupBy(b => new { b.Room.RoomName, b.Desk.DeskName })
                .Select(g => new
                {
                    RoomName = g.Key.RoomName,
                    DeskName = g.Key.DeskName,
                    TotalRevenue = g.Sum(b => b.TotalCost)
                })
                .ToListAsync();

            if (results == null || results.Count == 0)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No bookings found for the specified time period."
                });
            }

            var totalRevenue = results.Sum(r => r.TotalRevenue);
            var revenueByRoom = results
                .Where(r => r.DeskName == null)
                .ToDictionary(r => r.RoomName, r => r.TotalRevenue);

            var revenueByDesk = results
                .Where(r => r.DeskName != null)
                .ToDictionary(r => r.DeskName, r => r.TotalRevenue);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Revenue breakdown retrieved successfully.",
                TotalRevenue = totalRevenue,
                Currency = "EUR",
                RevenueByRoom = revenueByRoom,
                RevenueByDesk = revenueByDesk
            });
        }

        [HttpGet("admin/get-occupancy-report")]
        [Authorize(Roles = "Admin, CEO")]
        public async Task<IActionResult> GetOccupancyReport(
            [FromQuery] int CompanyId,
            [FromQuery] DateTime StartDate,
            [FromQuery] DateTime EndDate,
            [FromQuery] int? RoomId = null,
            [FromQuery] bool IncludeDesks = false)
        {
            // Input Validation
            if (StartDate >= EndDate)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Start date must be earlier than end date."
                });
            }

            // Verify Company Existence
            var company = await _context.Companies
                .Include(c => c.CompanyAddresses)
                .FirstOrDefaultAsync(c => c.CompanyId == CompanyId);

            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Company not found."
                });
            }

            // Retrieve Booking Data
            var bookingsQuery = _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .ThenInclude(d => d.Room)
                .Where(b => b.Room.CompanyAddress.CompanyId == CompanyId &&
                            b.StartTime >= StartDate &&
                            b.EndTime <= EndDate);

            if (RoomId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.RoomId == RoomId);
            }

            var bookings = await bookingsQuery.ToListAsync();

            if (!bookings.Any())
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No bookings found for the specified time period."
                });
            }

            // Calculate Occupancy
            var totalHours = (EndDate - StartDate).TotalHours;
            var roomOccupancy = bookings
                .Where(b => b.Room != null)
                .GroupBy(b => b.Room)
                .Select(g => new RoomOccupancyDto
                {
                    RoomId = g.Key.RoomId,
                    RoomName = g.Key.RoomName,
                    OccupancyPercentage = (g.Sum(b => (b.EndTime - b.StartTime).TotalHours) / totalHours) * 100
                })
                .OrderByDescending(o => o.OccupancyPercentage)
                .ToList();

            var deskOccupancy = new List<DeskOccupancyDto>();

            if (IncludeDesks)
            {
                deskOccupancy = bookings
                    .Where(b => b.Desk != null)
                    .GroupBy(b => b.Desk)
                    .Select(g => new DeskOccupancyDto
                    {
                        DeskId = g.Key.DeskId,
                        DeskName = g.Key.DeskName,
                        OccupancyPercentage = (g.Sum(b => (b.EndTime - b.StartTime).TotalHours) / totalHours) * 100
                    })
                    .OrderByDescending(o => o.OccupancyPercentage)
                    .ToList();
            }

            var overallOccupancyRate = roomOccupancy.Sum(r => r.OccupancyPercentage) / roomOccupancy.Count;

            var report = new OccupancyReportDto
            {
                OccupancyRate = overallOccupancyRate,
                TotalBookings = bookings.Count,
                MostUsedRoom = roomOccupancy.FirstOrDefault(),
                LeastUsedDesk = deskOccupancy.LastOrDefault()
            };

            // Generate Response
            return Ok(new
            {
                StatusCode = 200,
                Message = "Occupancy report generated successfully.",
                Data = report
            });
        }

        [HttpGet("get-booking-statistics-period")]
        [Authorize(Roles = "Admin, SuperAdmin, CEO")]
        public async Task<IActionResult> GetBookingStatisticsPeriod(
            [FromQuery] int CompanyId,
            [FromQuery] DateTime StartTime,
            [FromQuery] DateTime EndTime,
            [FromQuery] string? Username = null)
        {
            if (CompanyId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid CompanyId. Please provide a valid company."
                });
            }

            if (StartTime >= EndTime)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Start time must be earlier than end time."
                });
            }

            var company = await _context.Companies.FindAsync(CompanyId);
            if (company == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Company not found."
                });
            }

            string userId = null;
            if (!string.IsNullOrEmpty(Username))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == Username);
                if (user == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User not found."
                    });
                }
                userId = user.Id;
            }

            var bookingsQuery = _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .ThenInclude(d => d.Room)
                .Where(b => b.Room.CompanyAddress.CompanyId == CompanyId
                            && b.StartTime >= StartTime
                            && b.EndTime <= EndTime);

            if (!string.IsNullOrEmpty(userId))
            {
                bookingsQuery = bookingsQuery.Where(b => b.UserId == userId);
            }

            var bookings = await bookingsQuery.ToListAsync();

            if (bookings == null || !bookings.Any())
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No bookings found for the specified time period."
                });
            }

            // Group and calculate statistics
            var bookingStatistics = bookings
                .GroupBy(b => new { b.RoomId, b.Room.RoomName, b.DeskId, b.Desk.DeskName })
                .Select(g => new
                {
                    RoomId = g.Key.RoomId,
                    RoomName = g.Key.RoomName,
                    DeskId = g.Key.DeskId,
                    DeskName = g.Key.DeskName,
                    TotalBookings = g.Count(),
                    TotalRevenue = g.Sum(b => b.TotalCost)
                }).ToList();

            var response = new
            {
                StatusCode = 200,
                Message = "Booking statistics retrieved successfully.",
                CompanyId,
                StartTime,
                EndTime,
                TotalRevenue = bookingStatistics.Sum(b => b.TotalRevenue),
                TotalBookings = bookingStatistics.Sum(b => b.TotalBookings),
                Currency = "EUR",
                Breakdown = bookingStatistics
            };

            return Ok(response);
        }

        [HttpPost("book-desk-or-room")]
        [Authorize]
        public async Task<IActionResult> BookDeskOrRoom([FromBody] CreateBookingDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid input. Please provide the booking details."
                });
            }

            if (dto.CompanyId <= 0 || dto.RoomId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid company or room ID. Please check the inputs."
                });
            }

            if (dto.StartTime >= dto.EndTime)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Start time must be earlier than end time."
                });
            }

            // Extract userId from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "User identity cannot be determined."
                });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found."
                });
            }

            try
            {
                // Call the stored procedure to book the desk/room
                var outputMessage = new SqlParameter
                {
                    ParameterName = "@OutputMessage",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Size = 255,
                    Direction = System.Data.ParameterDirection.Output
                };

                var sql = "EXEC sp_BookDeskOrRoom @UserId, @CompanyId, @RoomId, @DeskId, @StartTime, @EndTime, @OutputMessage OUTPUT";

                var parameters = new[]
                {
                    new SqlParameter("@UserId", user.Id),
                    new SqlParameter("@CompanyId", dto.CompanyId),
                    new SqlParameter("@RoomId", dto.RoomId),
                    new SqlParameter("@DeskId", (object?)dto.DeskId ?? DBNull.Value),
                    new SqlParameter("@StartTime", dto.StartTime),
                    new SqlParameter("@EndTime", dto.EndTime),
                    outputMessage
                };

                await _context.Database.ExecuteSqlRawAsync(sql, parameters);

                var message = outputMessage.Value?.ToString();

                if (message == "Booking created successfully.")
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = message
                    });
                }
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An unexpected error occurred.",
                    Error = ex.Message
                });
            }
        }

        [HttpPut("update-my-booking")]
        [Authorize]
        public async Task<IActionResult> UpdateMyBooking([FromBody] UpdateBookingDto dto)
        {
            // Validate input DTO
            if (dto == null || dto.BookingId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid booking ID or input data."
                });
            }

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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found."
                });
            }

            // Fetch booking associated with the user and BookingId
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .FirstOrDefaultAsync(b => b.BookingId == dto.BookingId && b.UserId == user.Id);

            if (booking == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Booking not found or you are not authorized to update this booking."
                });
            }

            // Validate booking times
            if (dto.StartTime != null && dto.EndTime != null && dto.StartTime >= dto.EndTime)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Start time must be earlier than end time."
                });
            }

            // Update booking properties if provided
            if (dto.StartTime.HasValue) booking.StartTime = dto.StartTime.Value;
            if (dto.EndTime.HasValue) booking.EndTime = dto.EndTime.Value;
            if (dto.IsCancelled.HasValue) booking.IsCancelled = dto.IsCancelled.Value;
            if (!string.IsNullOrWhiteSpace(dto.CancellationReason)) booking.CancellationReason = dto.CancellationReason;
            if (dto.IsCheckedIn.HasValue) booking.IsCheckedIn = dto.IsCheckedIn.Value;

            // Update timestamp
            booking.UpdatedAt = DateTime.UtcNow;

            // Save changes
            await _context.SaveChangesAsync();

            // Map to DTO for response
            var updatedBookingDto = _mapper.Map<BookingDetailsDto>(booking);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Booking updated successfully.",
                Data = updatedBookingDto
            });
        }

        [HttpDelete("delete-booking")]
        [Authorize]
        public async Task<IActionResult> DeleteBooking([FromQuery] int bookingId)
        {
            // Check if BookingId is valid
            if (bookingId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid booking ID. Please provide a valid booking ID."
                });
            }

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
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found."
                });
            }

            // Check if the booking exists and belongs to the user
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.UserId == user.Id);

            if (booking == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Booking not found or does not belong to the user."
                });
            }

            // Remove the booking
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Booking deleted successfully.",
            });
        }
    }
}