using AutoMapper;
using CoworkingSpaceAPI.Dtos.Auth.Request;
using CoworkingSpaceAPI.Dtos.Auth.Response;
using CoworkingSpaceAPI.Models;
using CoworkingSpaceAPI.Services.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CoworkingSpaceAPI.Controllers
{
    // Define the route for the controller and mark it as an API controller
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        // Dependency injection for UserManager, RoleManager, and IJwtTokenService
        private readonly UserManager<ApplicationUserModel> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly CoworkingSpaceDbContext _context;
        private readonly IMapper _mapper;

        // Constructor to initialize the injected services
        public AccountController(UserManager<ApplicationUserModel> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenService jwtTokenService, CoworkingSpaceDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("admin/get-all-users")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            // Map to DTO and fetch roles
            var userDtos = new List<AdminUserDetailsDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userDto = _mapper.Map<AdminUserDetailsDto>(user);
                userDto.Roles = roles.ToList();  // Map roles separately

                userDtos.Add(userDto);
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "Users retrieved successfully.",
                Data = userDtos
            });
        }

        // Get user details using JWT token
        [HttpGet("user/get-user-details")]
        public async Task<IActionResult> GetUserDetails()
        {
            // Extract username from JWT token
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Username cannot be determined from the token."
                });
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"User with username '{username}' was not found."
                });
            }

            var roles = await _userManager.GetRolesAsync(user);

            var address = await _context.UserAddresses
                .Include(ua => ua.Address)
                .Include(ua => ua.AddressType)
                .Where(ua => ua.UserId == user.Id && (ua.IsDefault ?? false))
                .FirstOrDefaultAsync();

            var userWithDetails = _mapper.Map<UserDetailsDto>(user);

            if (address != null)
            {
                _mapper.Map(address.Address, userWithDetails);
                userWithDetails.AddressType = address.AddressType.AddressTypeName;
            }

            userWithDetails.Roles = roles.ToList();

            return Ok(new
            {
                StatusCode = 200,
                Message = "User found successfully.",
                Data = userWithDetails
            });
        }

        // Get user by username
        [HttpGet("admin/get-user-by-username/{username}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Username cannot be empty."
                });
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"User with username '{username}' was not found."
                });
            }

            var roles = await _userManager.GetRolesAsync(user);

            var address = await _context.UserAddresses
                .Include(ua => ua.Address)
                .Include(ua => ua.AddressType)
                .Where(ua => ua.UserId == user.Id && (ua.IsDefault ?? false))
                .FirstOrDefaultAsync();

            var userWithDetails = _mapper.Map<UserDetailsDto>(user);

            _mapper.Map(address.Address, userWithDetails);
            userWithDetails.AddressType = address.AddressType.AddressTypeName;

            userWithDetails.Roles = roles.ToList();

            return Ok(new
            {
                StatusCode = 200,
                Message = "User found successfully.",
                Data = userWithDetails
            });
        }

        // Admin deletes a user by username
        [HttpDelete("admin/delete-user/{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username); // Find the user by username
            if (user == null)
                return NotFound($"User with username '{username}' not found."); // Return not found if user doesn't exist

            var result = await _userManager.DeleteAsync(user); // Attempt to delete the user
            if (result.Succeeded)
                return Ok("User deleted successfully."); // Return success if deletion is successful

            // Return any errors encountered during deletion
            return BadRequest(result.Errors.Select(e => e.Description));
        }

        // Allows a user to delete their own account
        [HttpDelete("delete-my-account")]
        public async Task<IActionResult> DeleteMyAccount()
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
                    Message = "User not found."
                });
            }

            // Attempt to delete the user
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Your account has been deleted successfully."
                });
            }

            // Return any errors encountered during deletion
            return BadRequest(new
            {
                StatusCode = 400,
                Message = "An error occurred while deleting the account.",
                Errors = result.Errors.Select(e => e.Description).ToList()
            });
        }

        // Deletes all users (function similar to an admin's action but without authorization required)
        [HttpDelete("public-like-admin/delete-all-users")]
        public async Task<IActionResult> DeleteAllUsers()
        {
            var users = _userManager.Users.ToList(); // Get all users

            // Iterate through each user and delete them
            foreach (var user in users)
            {
                var result = await _userManager.DeleteAsync(user); // Delete the user
                if (!result.Succeeded)
                    return BadRequest($"Failed to delete user {user.UserName}"); // Return error if deletion fails
            }

            return Ok("All users deleted successfully."); // Return success after all users are deleted
        }

        [HttpPut("user/update-profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserDetailsUpdatedDto model)
        {
            var username = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new { StatusCode = 400, Message = "Username cannot be determined from the token." });
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound(new { StatusCode = 404, Message = "User not found." });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Map UserDetailsDto to user entity
                _mapper.Map(model, user);

                var userUpdateResult = await _userManager.UpdateAsync(user);
                if (!userUpdateResult.Succeeded)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = $"Failed to update user profile: {string.Join(", ", userUpdateResult.Errors.Select(e => e.Description))}"
                    });
                }

                // Map Address if provided
                if (model.Street != null || model.City != null || model.Country != null || model.AddressType != null)
                {
                    var address = await _context.UserAddresses
                        .Include(ua => ua.Address)
                        .Include(ua => ua.AddressType)
                        .FirstOrDefaultAsync(ua => ua.UserId == user.Id && (ua.IsDefault ?? false))
                        ?? new UserAddress { UserId = user.Id, Address = new Address(), IsDefault = true, CreatedAt = DateTime.UtcNow };

                    _mapper.Map(model, address.Address);
                    if (!string.IsNullOrWhiteSpace(model.AddressType))
                    {
                        var addressType = await _context.AddressTypes.FirstOrDefaultAsync(at => at.AddressTypeName == model.AddressType)
                            ?? new AddressType { AddressTypeName = model.AddressType };
                        address.AddressType = addressType;
                    }
                    address.UpdatedAt = DateTime.UtcNow;

                    _context.UserAddresses.Update(address);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { StatusCode = 200, Message = "User profile updated successfully." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { StatusCode = 500, Message = $"An error occurred: {ex.Message}" });
            }
        }

        // Admin updates a user by username (+ role)
        [HttpPut("admin/update-user/{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminUpdateUser(string username, [FromBody] AdminUpdateUserDto model)
        {
            var user = await _userManager.FindByNameAsync(username); // Find the user by username
            if (user == null)
                return NotFound($"User with username '{username}' not found."); // Return not found if user doesn't exist

            // Update user properties based on input model
            user.UserName = model.Username;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            var updateResult = await _userManager.UpdateAsync(user); // Attempt to update the user
            if (!updateResult.Succeeded)
                return BadRequest($"Error updating user: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}"); // Return any errors

            // Handle password update if provided
            if (!string.IsNullOrEmpty(model.Password))
            {
                var passwordRemovalResult = await _userManager.RemovePasswordAsync(user); // Remove current password
                if (passwordRemovalResult.Succeeded)
                {
                    var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password); // Add new password
                    if (!addPasswordResult.Succeeded)
                        return BadRequest($"Error setting password: {string.Join(", ", addPasswordResult.Errors.Select(e => e.Description))}"); // Return any errors
                }
                else
                {
                    return BadRequest($"Error removing password: {string.Join(", ", passwordRemovalResult.Errors.Select(e => e.Description))}"); // Return any errors
                }
            }

            // Handle role update if provided
            if (!string.IsNullOrEmpty(model.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user); // Get current roles
                await _userManager.RemoveFromRolesAsync(user, currentRoles); // Remove from current roles

                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role); // Add new role if it exists
                }
                else
                {
                    return BadRequest($"Role '{model.Role}' does not exist."); // Return error if role doesn't exist
                }
            }

            // Return success after the update
            return Ok($"User '{username}' updated successfully by admin.");
        }

        // Change a user's role (accessible without Admin role but performs similar action)
        [HttpPut("public-like-admin/add-role")]
        public async Task<IActionResult> AddUserRole([FromBody] ChangeUserRoleRequestDto model)
        {
            // Validate input for username and role
            if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.RoleName))
                return BadRequest("Invalid input.");

            var user = await _userManager.FindByNameAsync(model.Username); // Find user by username
            if (user == null)
                return NotFound("User not found."); // Return not found if user doesn't exist

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                return BadRequest($"Role does not exist.");
            }

            var userRoleExists = await _userManager.IsInRoleAsync(user, model.RoleName);
            if (userRoleExists)
            {
                return BadRequest($"User already has the role.");
            }

            // Add the new role
            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (!result.Succeeded)
                return BadRequest("Failed to add role to user."); // Return error if role add fails

            // Return success after role add
            return Ok($"Role {model.RoleName} added to user successfully.");
        }

        // Change a user's role (accessible without Admin role but performs similar action)
        [HttpPut("public-like-admin/remove-role")]
        public async Task<IActionResult> RemoveUserRole([FromBody] ChangeUserRoleRequestDto model)
        {
            // Validate input for username and role
            if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.RoleName))
                return BadRequest("Invalid input.");

            var user = await _userManager.FindByNameAsync(model.Username); // Find user by username
            if (user == null)
                return NotFound("User not found."); // Return not found if user doesn't exist

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                return BadRequest($"Role does not exist.");
            }

            var userRoleExists = await _userManager.IsInRoleAsync(user, model.RoleName);
            if (!userRoleExists)
            {
                return BadRequest($"User does not have the role.");
            }

            // Add the new role
            var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

            if (!result.Succeeded)
                return BadRequest("Failed to remove user role."); // Return error if role remove fails

            // Return success after role remove
            return Ok($"User role removed to {model.RoleName} successfully.");
        }
    }
}