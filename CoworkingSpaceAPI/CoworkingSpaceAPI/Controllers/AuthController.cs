using CoworkingSpaceAPI.Dtos.Auth.Request; // Import the request DTOs for authentication
using CoworkingSpaceAPI.Models; // Import the ApplicationUserModel for user-related actions
using CoworkingSpaceAPI.Services.JwtToken; // Import the JwtTokenService for handling JWT tokens
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // Import Identity namespace for user and role management
using Microsoft.AspNetCore.Mvc; // Import namespace for ASP.NET Core MVC functionality
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CoworkingSpaceAPI.Controllers // Define the namespace for the AuthController
{
    [Route("api/[controller]")] // Define the route template for this controller
    [ApiController] // Mark this class as an API controller
    public class AuthController : ControllerBase // Inherit from ControllerBase for handling API requests
    {
        private readonly UserManager<ApplicationUserModel> _userManager; // Dependency injection for managing users
        private readonly IJwtTokenService _jwtTokenService; // Dependency injection for handling JWT token generation
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUserModel> userManager, IJwtTokenService jwtTokenService, IConfiguration configuration) // Constructor to inject the services
        {
            _userManager = userManager; // Initialize UserManager
            _jwtTokenService = jwtTokenService; // Initialize JwtTokenService
            _configuration = configuration;
        }

        [HttpPost("register-user")] // Define an HTTP POST endpoint for registering a user
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model) // Action method for user registration
        {
            if (!ModelState.IsValid) // Check if the model state is valid
                return BadRequest("Invalid registration details."); // Return bad request if validation fails

            var userExists = await _userManager.FindByNameAsync(model.Username); // Check if the username already exists
            if (userExists != null) // If user exists
                return Conflict("Username already exists."); // Return conflict response if username exists

            var emailExists = await _userManager.FindByEmailAsync(model.Email); // Check if the email is already in use
            if (emailExists != null) // If email exists
                return Conflict("Email is already in use."); // Return conflict response if email exists

            var user = new ApplicationUserModel // Create a new ApplicationUserModel instance
            {
                UserName = model.Username, // Set the username
                Email = model.Email, // Set the email
                FirstName = model.FirstName, // Set the first name
                LastName = model.LastName // Set the last name
            };

            // Create the user and hash the password
            var result = await _userManager.CreateAsync(user, model.Password); // Create the user with the provided password

            if (!result.Succeeded) // If user creation fails
                return BadRequest(result.Errors); // Return bad request with the errors

            // Assign 'NormalUser' role by default
            var roleResult = await _userManager.AddToRoleAsync(user, "NormalUser"); // Add the user to the 'NormalUser' role
            if (!roleResult.Succeeded) // If role assignment fails
                return BadRequest("Failed to assign role."); // Return bad request

            return Ok("User registered and assigned role successfully."); // Return success message on successful registration
        }

        [HttpPost("user-login")] // Define an HTTP POST endpoint for user login
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model) // Action method for user login
        {
            if (!ModelState.IsValid) // Check if the model state is valid
                return BadRequest("Invalid login details. Please provide a valid username and password."); // Return bad request if validation fails

            var user = await _userManager.FindByNameAsync(model.Username); // Find the user by their username
            if (user == null) // If user does not exist
                return Unauthorized("Username does not exist."); // Return unauthorized response if username is not found

            if (!await _userManager.CheckPasswordAsync(user, model.Password)) // Check if the provided password is correct
                return Unauthorized("Incorrect password."); // Return unauthorized response if password is incorrect

            // Generate token using the JWT token service
            var token = await _jwtTokenService.GenerateToken(user); // Await the task for token generation

            if (string.IsNullOrEmpty(token)) // If token generation fails
                return StatusCode(500, "An error occurred while generating the token."); // Return internal server error

            var userRoles = await _userManager.GetRolesAsync(user); // Retrieve the roles assigned to the user
            return Ok(new { Token = token, Message = "Login successful." }); // Return success response with the token and roles
        }

        // Endpoint to verify the validity of a JWT
        [HttpGet("validate-jwt")]
        [Authorize]
        public IActionResult ValidateJwt()
        {
            try
            {
                // Extract the token from the Authorization header
                var authHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Authorization header is missing or invalid."
                    });
                }

                var token = authHeader.Substring(7); // Remove "Bearer " from the header
                var tokenHandler = new JwtSecurityTokenHandler();

                // Decode and validate the token
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"], // Read from appsettings.json
                    ValidAudience = _configuration["Jwt:Audience"], // Read from appsettings.json
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Extract claims and return success
                var jwtToken = validatedToken as JwtSecurityToken;
                var username = jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "JWT is valid.",
                    Username = username
                });
            }
            catch (SecurityTokenExpiredException)
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "JWT has expired. Please log in again."
                });
            }
            catch (SecurityTokenException)
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "JWT is invalid. Please provide a valid token."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while validating the token.",
                    Details = ex.Message
                });
            }
        }
    }
}