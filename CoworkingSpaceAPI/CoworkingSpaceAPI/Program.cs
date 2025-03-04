using CoworkingSpaceAPI.Models;
using CoworkingSpaceAPI.Services.JwtToken;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity; // Import Identity for user and role management
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text; // Import for encoding JWT secret key

var builder = WebApplication.CreateBuilder(args); // Create a builder for the web application
builder.Configuration.AddEnvironmentVariables(); // Add environment variables to the configuration

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  // Registers all profiles in the project

// Load environment variables from .env file
Env.Load(".env");

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Listen(System.Net.IPAddress.Any, 7198);
//});

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; // Optional: For better formatting in responses
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer(); // Add API explorer for endpoints

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    // Configure JWT Authorization in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer prefix 'Bearer {token}': ",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] { }
        }
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalNetwork", // Define a CORS policy named "AllowLocalNetwork"
        builder =>
        {
            builder.WithOrigins("http://localhost:3050") // Allow specific frontend URL origin
                   .AllowAnyHeader()    // Allow any headers in CORS requests
                   .AllowAnyMethod()    // Allow any HTTP methods
                   .AllowCredentials(); // Allow cookies to be sent
        });
});

var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
dbConnectionString = dbConnectionString.Replace("${DB_SERVER_IP}", Environment.GetEnvironmentVariable("DB_SERVER_IP"));
var dbServerIp = Environment.GetEnvironmentVariable("DB_SERVER_IP");

if (dbConnectionString != null && dbServerIp != null)
{
    dbConnectionString = dbConnectionString.Replace("${DB_SERVER_IP}", dbServerIp);
}
else
{
    // Handle the case where either dbConnectionString or dbServerIp is null
    Console.WriteLine("Either dbConnectionString or DB_SERVER_IP is not set.");
    throw new InvalidOperationException("Database connection string 'DefaultConnection' is not configured.");
}

// Configure the database context
builder.Services.AddDbContext<CoworkingSpaceDbContext>(options =>
options.UseSqlServer(dbConnectionString)); // Configure SQL Server with a connection string

// Configure Identity
builder.Services.AddIdentity<ApplicationUserModel, IdentityRole>() // Add Identity services for ApplicationUserModel and roles
                                                                   .AddEntityFrameworkStores<CoworkingSpaceDbContext>() // Use the database context for storing identity data
    .AddDefaultTokenProviders(); // Add default token providers for password reset and other identity features

// Configure JWT Key, Issuer, and Audience from environment variables
builder.Configuration["Jwt:Key"] = Env.GetString("JWT_KEY") ?? throw new InvalidOperationException("JWT Key is missing"); // Get JWT key from .env

// JWT Config
var jwtKey = builder.Configuration["Jwt:Key"]; // Store the JWT key in a variable
var jwtIssuer = builder.Configuration["Jwt:Issuer"]; // Store the JWT issuer in a variable
var jwtAudience = builder.Configuration["Jwt:Audience"]; // Store the JWT audience in a variable

if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT Key is not configured in the settings.");
}

// JWT Validation params
var keyBytes = Encoding.UTF8.GetBytes(jwtKey); // Convert the JWT key into a byte array
var tokenValidationParams = new TokenValidationParameters // Set up token validation parameters
{
    ValidateIssuerSigningKey = true, // Ensure the token is signed with the correct key
    IssuerSigningKey = new SymmetricSecurityKey(keyBytes), // Use the symmetric security key for validation
    ValidateIssuer = true, // Validate the token's issuer
    ValidIssuer = jwtIssuer, // Set the valid issuer
    ValidateAudience = true, // Validate the token's audience
    ValidAudience = jwtAudience, // Set the valid audience
    ValidateLifetime = true, // Ensure the token has not expired
    RoleClaimType = ClaimTypes.Role, // Map roles from the JWT
    NameClaimType = JwtRegisteredClaimNames.Sub,  // Map user ID from the JWT
};

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Set the default authentication scheme to JWT Bearer
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Set the default challenge scheme to JWT Bearer
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParams; // Use the defined token validation parameters
    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
    options.Events = new JwtBearerEvents // Set up event handling for JWT Bearer authentication
    {
        OnAuthenticationFailed = context =>
        {
            context.Response.Headers.Append("Token-Error", "Invalid token"); // Add a custom header for token errors
            return Task.CompletedTask; // Complete the task after handling the error
        },
        OnTokenValidated = context =>
        {
            // You can add additional validation logic here if needed
            return Task.CompletedTask; // Complete the task after token validation
        }
    };
});

// Authorization Policies
builder.Services.AddAuthorizationBuilder()
                             .AddPolicy("SuperAdmin", policy => policy.RequireRole("SuperAdmin"))
                             .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
                             .AddPolicy("NormalUser", policy => policy.RequireRole("NormalUser"))
                             .AddPolicy("TeamLead", policy => policy.RequireRole("TeamLead"))
                             .AddPolicy("Guest", policy => policy.RequireRole("Guest"))
                             .AddPolicy("Moderator", policy => policy.RequireRole("Moderator"))
                             .AddPolicy("Developer", policy => policy.RequireRole("Developer"))
                             .AddPolicy("Tester", policy => policy.RequireRole("Tester"))
                             .AddPolicy("DataScientist", policy => policy.RequireRole("DataScientist"))
                             .AddPolicy("DataScientist", policy => policy.RequireRole("CEO"));

// Add Scoped services
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>(); // Add JwtTokenService to the service collection with Scoped lifetime

var app = builder.Build(); // Build the application

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment()) // Check if the application is in development environment
{
    app.UseSwagger(); // Enable Swagger for API documentation
    app.UseSwaggerUI(); // Enable the Swagger UI
    app.UseDeveloperExceptionPage(); // Enables detailed error pages
}

// Create roles on startup
using (var scope = app.Services.CreateScope()) // Create a scope for dependency injection
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); // Get the RoleManager service
    var roles = new[] { "Admin", "NormalUser", "SuperAdmin", "Moderator", "TeamLead", "Developer", "Tester", "Guest", "DataScientist", "CEO" }; // Define a list of roles
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role)) // Check if the role exists
        {
            await roleManager.CreateAsync(new IdentityRole(role)); // Create the role if it doesn't exist
        }
    }
}

// Add the middleware to the pipeline
app.UseHttpsRedirection(); // Redirect all HTTP requests to HTTPS

app.UseCors("AllowLocalNetwork"); // use the CORS policy defined earlier

app.UseAuthentication(); // Enable Authentication middleware
app.UseAuthorization(); // Enable Authorization middleware

app.MapControllers(); // Map controller routes

app.Run(); // Run the application