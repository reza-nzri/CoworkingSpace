using AutoMapper;
using CoworkingSpaceAPI.Dtos.Auth.Request;
using CoworkingSpaceAPI.Models;
using CoworkingSpaceAPI.Services.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // Get all users and their roles
        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync(); // Retrieve all users
            var usersWithRoles = new List<object>(); // Create a list to hold users with roles

            // Iterate through each user and retrieve their roles
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Get roles for the user

                // Create an anonymous object containing the user's details and roles
                var userWithRoles = new
                {
                    user.FirstName,
                    user.LastName,
                    user.Id,
                    user.UserName,
                    user.NormalizedUserName,
                    user.Email,
                    user.NormalizedEmail,
                    user.EmailConfirmed,
                    user.PasswordHash,
                    user.SecurityStamp,
                    user.ConcurrencyStamp,
                    user.PhoneNumber,
                    user.PhoneNumberConfirmed,
                    user.TwoFactorEnabled,
                    user.LockoutEnd,
                    user.LockoutEnabled,
                    user.AccessFailedCount,
                    Roles = roles.ToList() // Convert roles to a list
                };

                usersWithRoles.Add(userWithRoles); // Add the user with roles to the list
            }

            // Return the list of users with their roles
            return Ok(usersWithRoles);
        }

        // Get user by username
        [HttpGet("get-user-by-username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Username cannot be empty."
                });
            }

            // Find user by username
            var user = await _userManager.FindByNameAsync(username);

            // Check if user exists
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"User with username '{username}' was not found."
                });
            }

            // Get roles for the user
            var roles = await _userManager.GetRolesAsync(user);

            // Construct user response with roles
            var userWithRoles = new
            {
                user.FirstName,
                user.LastName,
                user.Id,
                user.UserName,
                user.NormalizedUserName,
                user.Email,
                user.NormalizedEmail,
                user.EmailConfirmed,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.TwoFactorEnabled,
                user.LockoutEnd,
                user.LockoutEnabled,
                user.AccessFailedCount,
                Roles = roles.ToList() // Convert roles to a list
            };

            // Return success response with user data
            return Ok(new
            {
                StatusCode = 200,
                Message = "User found successfully.",
                Data = userWithRoles
            });
        }

        [HttpGet("get-user-details")]
        public async Task<IActionResult> GetUserDetails()
        {
            // Extract username from JWT
            var username = User?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Username cannot be determined from the token."
                });
            }

            // Find user by username
            var user = await _userManager.FindByNameAsync(username);

            // Check if user exists
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"User with username '{username}' was not found."
                });
            }

            // Get roles for the user
            var roles = await _userManager.GetRolesAsync(user);

            // Construct user response including all fields from UserProfileUpdateDto
            var userWithRolesAndDetails = new UserProfileUpdateDto
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Prefix = user.Prefix,
                Suffix = user.Suffix,
                Nickname = user.Nickname,
                RecoveryEmail = user.RecoveryEmail,
                AlternaiveEmail = user.AlternaiveEmail,
                RecoveryPhoneNumber = user.RecoveryPhoneNumber,
                Gender = user.Gender,
                Birthday = user.Birthday,
                ProfilePicturePath = user.ProfilePicturePath,
                CompanyName = user.CompanyName,
                JobTitle = user.JobTitle,
                Department = user.Department,
                AppLanguage = user.AppLanguage,
                Website = user.Website,
                Linkedin = user.Linkedin,
                Facebook = user.Facebook,
                Instagram = user.Instagram,
                Twitter = user.Twitter,
                Github = user.Github,
                Youtube = user.Youtube,
                Tiktok = user.Tiktok,
                Snapchat = user.Snapchat,
            };

            // Query the address associated with the user
            var address = await _context.UserAddresses
                .Where(ua => ua.UserId == user.Id && (ua.IsDefault ?? false))
                .Select(ua => new
                {
                    ua.Address.Street,
                    ua.Address.HouseNumber,
                    ua.Address.PostalCode,
                    ua.Address.City,
                    ua.Address.State,
                    ua.Address.Country,
                    AddressType = ua.AddressType.AddressTypeName,
                    ua.IsDefault
                })
                .FirstOrDefaultAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "User found successfully.",
                Data = new
                {
                    User = userWithRolesAndDetails,
                    Address = address
                }
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
        public async Task<IActionResult> DeleteMyAccount([FromBody] string username)
        {
            var user = await _userManager.FindByNameAsync(username); // Find the user by username
            if (user == null)
                return NotFound("User not found."); // Return not found if user doesn't exist

            var result = await _userManager.DeleteAsync(user); // Attempt to delete the user
            if (result.Succeeded)
                return Ok("Your account has been deleted successfully."); // Return success if deletion is successful

            // Return any errors encountered during deletion
            return BadRequest(result.Errors.Select(e => e.Description));
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

        [HttpPut("update-my-profile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UserProfileUpdateDto model)
        {
            // Extract the username from the JWT
            var username = User?.Identity?.Name; // Assuming the username is stored in the Name claim of the JWT
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Invalid token or username not found.");
            }

            // Find the user using the username extracted from the JWT
            var user = await _userManager.FindByNameAsync(username); // Change from model.Username to username
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // to allow updating the username if it's different and doesn't already exist in the database
            if (!string.IsNullOrEmpty(model.Username) && model.Username != user.UserName)
            {
                var existingUser = await _userManager.FindByNameAsync(model.Username);
                if (existingUser != null)
                {
                    return Conflict("The chosen username is already in use. Please choose a different one.");
                }
                user.UserName = model.Username; // Assign the new username
                user.NormalizedUserName = model.Username.ToUpper(); // Keep the normalized username updated
            }

            // Begin transaction for atomic updates
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update user properties
                user.Email = model.Email switch { null => null, "" => user.Email, _ => model.Email };
                user.FirstName = model.FirstName switch { null => null, "" => user.FirstName, _ => model.FirstName };
                user.MiddleName = model.MiddleName switch { null => null, "" => user.MiddleName, _ => model.MiddleName };
                user.LastName = model.LastName switch { null => null, "" => user.LastName, _ => model.LastName };
                user.Prefix = model.Prefix switch { null => null, "" => user.Prefix, _ => model.Prefix };
                user.Suffix = model.Suffix switch { null => null, "" => user.Suffix, _ => model.Suffix };
                user.Nickname = model.Nickname switch { null => null, "" => user.Nickname, _ => model.Nickname };
                user.RecoveryEmail = model.RecoveryEmail switch { null => null, "" => user.RecoveryEmail, _ => model.RecoveryEmail };
                user.AlternaiveEmail = model.AlternaiveEmail switch { null => null, "" => user.AlternaiveEmail, _ => model.AlternaiveEmail };
                user.RecoveryPhoneNumber = model.RecoveryPhoneNumber switch { null => null, "" => user.RecoveryPhoneNumber, _ => model.RecoveryPhoneNumber };
                user.Gender = model.Gender switch { null => null, "" => user.Gender, _ => model.Gender };
                user.Birthday = model.Birthday.HasValue ? model.Birthday.Value : null;
                user.ProfilePicturePath = model.ProfilePicturePath switch { null => null, "" => user.ProfilePicturePath, _ => model.ProfilePicturePath };
                user.CompanyName = model.CompanyName switch { null => null, "" => user.CompanyName, _ => model.CompanyName };
                user.JobTitle = model.JobTitle switch { null => null, "" => user.JobTitle, _ => model.JobTitle };
                user.Department = model.Department switch { null => null, "" => user.Department, _ => model.Department };
                user.AppLanguage = model.AppLanguage switch { null => null, "" => user.AppLanguage, _ => model.AppLanguage };
                user.Website = model.Website switch { null => null, "" => user.Website, _ => model.Website };
                user.Linkedin = model.Linkedin switch { null => null, "" => user.Linkedin, _ => model.Linkedin };
                user.Facebook = model.Facebook switch { null => null, "" => user.Facebook, _ => model.Facebook };
                user.Instagram = model.Instagram switch { null => null, "" => user.Instagram, _ => model.Instagram };
                user.Twitter = model.Twitter switch { null => null, "" => user.Twitter, _ => model.Twitter };
                user.Github = model.Github switch { null => null, "" => user.Github, _ => model.Github };
                user.Youtube = model.Youtube switch { null => null, "" => user.Youtube, _ => model.Youtube };
                user.Tiktok = model.Tiktok switch { null => null, "" => user.Tiktok, _ => model.Tiktok };
                user.Snapchat = model.Snapchat switch { null => null, "" => user.Snapchat, _ => model.Snapchat };
                user.UpdatedAt = DateTime.Now;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return BadRequest($"Error updating your profile: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
                }

                // Handle password update
                if (!string.IsNullOrEmpty(model.Password))
                {
                    var passwordRemovalResult = await _userManager.RemovePasswordAsync(user);
                    if (!passwordRemovalResult.Succeeded)
                    {
                        return BadRequest($"Error removing your password: {string.Join(", ", passwordRemovalResult.Errors.Select(e => e.Description))}");
                    }

                    var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                    if (!addPasswordResult.Succeeded)
                    {
                        return BadRequest($"Error setting your password: {string.Join(", ", addPasswordResult.Errors.Select(e => e.Description))}");
                    }
                }

                // Handle address update
                if (model.Street != null || model.City != null || model.Country != null || model.AddressType != null)
                {
                    var userAddress = await _context.UserAddresses
                        .Include(ua => ua.Address)
                        .Include(ua => ua.AddressType)
                        .FirstOrDefaultAsync(ua => ua.UserId == user.Id && ua.IsDefault == true);

                    var address = userAddress?.Address ?? new Address();
                    address.Street = model.Street switch { null => address.Street, "" => null, _ => model.Street };
                    address.HouseNumber = model.HouseNumber switch { null => address.HouseNumber, "" => null, _ => model.HouseNumber };
                    address.PostalCode = model.PostalCode switch { null => address.PostalCode, "" => null, _ => model.PostalCode };
                    address.City = model.City switch { null => address.City, "" => null, _ => model.City };
                    address.State = model.State switch { null => address.State, "" => null, _ => model.State };
                    address.Country = model.Country switch { null => address.Country, "" => null, _ => model.Country };

                    if (userAddress == null)
                    {
                        var addressType = await _context.AddressTypes.FirstOrDefaultAsync(at => at.AddressTypeName == model.AddressType)
                                          ?? new AddressType { AddressTypeName = model.AddressType, Description = $"Type for {model.AddressType}" };

                        if (addressType.AddressTypeId == 0)
                        {
                            _context.AddressTypes.Add(addressType);
                            await _context.SaveChangesAsync();
                        }

                        userAddress = new UserAddress
                        {
                            UserId = user.Id,
                            Address = address,
                            AddressType = addressType,
                            IsDefault = model.IsDefaultAddress ?? true,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        _context.UserAddresses.Add(userAddress);
                    }
                    else
                    {
                        var addressType = await _context.AddressTypes.FirstOrDefaultAsync(at => at.AddressTypeName == model.AddressType)
                                          ?? userAddress.AddressType;

                        userAddress.AddressType = addressType;
                        userAddress.UpdatedAt = DateTime.Now;
                        _context.Addresses.Update(address);
                    }

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return Ok("Your profile has been updated successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
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