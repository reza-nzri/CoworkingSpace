using CoworkingSpaceAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoworkingSpaceAPI.Configs
{
    public static class Seeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUserModel>>();
            var dbContext = serviceProvider.GetRequiredService<CoworkingSpaceDbContext>();

            // Fetch the admin password from the environment variables
            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            // Überprüfung, ob Benutzer bereits existieren
            var userExists = await dbContext.Users.AnyAsync();
            if (!userExists) // Wenn keine Benutzer vorhanden sind
            {
                // Seed Users
                var users = new[]
                {
                    new { Username = "admin", Email = "admin@exsample.com", Password = adminPassword, FirstName = "AdminF", LastName = "AdminL", Gender = "M", Birthday = "1990-01-01", Role = "Admin" },
                    new { Username = "john_doe", Email = "john.doe@email.com", Password = "JohnDoe@123", FirstName = "John", LastName = "Doe", Gender = "M", Birthday = "1985-05-15", Role = "NormalUser" },
                    new { Username = "sam_smith", Email = "sam.smith@email.com", Password = "SamSmith@123", FirstName = "Sam", LastName = "Smith", Gender = "O", Birthday = "1988-09-09", Role = "NormalUser" },
                    new { Username = "alice_wonder", Email = "alice.wonder@example.com", Password = "AliceWonder@123", FirstName = "Alice", LastName = "Wonder", Gender = "F", Birthday = "1982-06-15", Role = "NormalUser" },
                };

                foreach (var user in users)
                {
                    if (await userManager.FindByEmailAsync(user.Email) == null)
                    {
                        var newUser = new ApplicationUserModel
                        {
                            UserName = user.Username,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Gender = user.Gender,
                            Birthday = DateOnly.FromDateTime(DateTime.Parse(user.Birthday.ToString())),
                            EmailConfirmed = true,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            Status = "active"
                        };
                        var result = await userManager.CreateAsync(newUser, user.Password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newUser, user.Role);
                        }
                    }
                }
            }

            // Seed Address Types
            if (!dbContext.AddressTypes.Any())
            {
                var addressTypes = new List<AddressType>
                {
                    new AddressType { AddressTypeName = "Home", Description = "Residential address for home deliveries and personal correspondence." },
                    new AddressType { AddressTypeName = "Work", Description = "Workplace address used for business correspondence and deliveries." },
                    new AddressType { AddressTypeName = "Billing", Description = "Address used specifically for billing and financial statements." },
                    new AddressType { AddressTypeName = "Shipping", Description = "Address used for shipping goods and receiving parcels." },
                    new AddressType { AddressTypeName = "Headquarters", Description = "Primary office location for corporate activities." },
                    new AddressType { AddressTypeName = "Branch", Description = "Secondary office location used for local operations." },
                    new AddressType { AddressTypeName = "Warehouse", Description = "Storage location for inventory and logistical operations." },
                    new AddressType { AddressTypeName = "Factory", Description = "Industrial site used for manufacturing products." },
                    new AddressType { AddressTypeName = "R&D", Description = "Research and development facility address." },
                    new AddressType { AddressTypeName = "Field Office", Description = "Remote office location for field operations." }
                };

                dbContext.AddressTypes.AddRange(addressTypes);
                await dbContext.SaveChangesAsync();
            }

            // Seed Company
            if (!dbContext.Companies.Any())
            {
                // Seed AddressType
                var addressType = new AddressType { AddressTypeName = "Headquarters", Description = "Main office location" };
                dbContext.AddressTypes.Add(addressType);
                await dbContext.SaveChangesAsync();

                // Seed Address
                var address = new Address
                {
                    Street = "123 Tech Lane",
                    HouseNumber = "5A",
                    PostalCode = "90001",
                    City = "Techville",
                    State = "Innovate",
                    Country = "Futuria"
                };
                dbContext.Addresses.Add(address);
                await dbContext.SaveChangesAsync();

                // Seed Company
                var company = new Company
                {
                    Name = "Doe Enterprises",
                    Industry = "Technology",
                    Description = "Innovative tech solutions provider specializing in consumer electronics.",
                    RegistrationNumber = "REG0012345",
                    TaxId = "TX1234567",
                    Website = "https://doeenterprises.example.com",
                    ContactEmail = "info@doeenterprises.example.com",
                    ContactPhone = "+1234567890",
                    FoundedDate = DateOnly.FromDateTime(DateTime.Parse("2023-01-01")),
                    CompanyAddresses = new List<CompanyAddress>
                    {
                        new CompanyAddress
                        {
                            Address = address,
                            AddressType = addressType,
                            IsDefault = true,
                            CreatedAt = DateTime.Now
                        }
                    }
                };

                dbContext.Companies.Add(company);
                await dbContext.SaveChangesAsync();

                // Seed CEO
                var ceoUser = await userManager.FindByNameAsync("john_doe");
                if (ceoUser != null)
                {
                    dbContext.CompanyCeos.Add(new CompanyCeo
                    {
                        CompanyId = company.CompanyId,
                        CeoUserId = ceoUser.Id,
                        StartDate = DateOnly.FromDateTime(DateTime.Parse("2023-01-01"))
                    });
                    await dbContext.SaveChangesAsync();
                }

                // Seed Company Employee
                var existingCompany = dbContext.Companies.FirstOrDefault(c => c.Name == "Doe Enterprises");
                if (existingCompany != null)
                {
                    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "alice_wonder");
                    if (user != null && !dbContext.CompanyEmployees.Any(ce => ce.UserId == user.Id))
                    {
                        var companyEmployee = new CompanyEmployee
                        {
                            CompanyId = existingCompany.CompanyId,
                            UserId = user.Id,
                            StartDate = DateOnly.FromDateTime(DateTime.Parse("2023-01-01")),
                            Position = "Software Developer"
                        };
                        dbContext.CompanyEmployees.Add(companyEmployee);
                        await dbContext.SaveChangesAsync();
                    }
                }

                // Seed Rooms
                var rooms = Enumerable.Range(108, 7).Select(i => new Room
                {
                    RoomName = $"Raum {i}",
                    RoomType = "Workspace",
                    Price = 100.00M,
                    Currency = "EUR",
                    IsActive = i != 109,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CompanyAddressId = company.CompanyId
                }).ToList();

                dbContext.Rooms.AddRange(rooms);
                await dbContext.SaveChangesAsync();

                // Seed Desks
                foreach (var room in rooms)
                {
                    // Exclude "Raum 112" from having desks
                    if (room.RoomName != "Raum 112")
                    {
                        var desks = Enumerable.Range(1, room.RoomName == "Raum 108" ? 20 : 1).Select(j => new Desk
                        {
                            DeskName = $"AP{j:00}",
                            Price = 5.00M,
                            Currency = "EUR",
                            IsAvailable = true,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            RoomId = room.RoomId
                        }).ToList();

                        dbContext.Desks.AddRange(desks);
                    }
                }
                await dbContext.SaveChangesAsync();

                // Seed Labels and Assignments
                if (!dbContext.Labels.Any())
                {
                    var labels = new List<Label>
                    {
                        new Label { LabelName = "Eco Friendly", Description = "Environmentally friendly room", ColorCode = "#00FF00" },
                        new Label { LabelName = "VIP", Description = "Room for important meetings", ColorCode = "#FFD700" },
                        new Label { LabelName = "Reserved", Description = "Reserved for specific employees", ColorCode = "#FF4500" },
                        new Label { LabelName = "Public", Description = "Available for any employee", ColorCode = "#1E90FF" }
                    };
                    dbContext.Labels.AddRange(labels);
                    await dbContext.SaveChangesAsync();

                    // Cache label IDs
                    var labelDict = labels.ToDictionary(l => l.LabelName, l => l.LabelId);
                    var roomDict = dbContext.Rooms.Where(r => new[] { "Raum 114", "Raum 113" }.Contains(r.RoomName)).ToDictionary(r => r.RoomName, r => r.RoomId);
                    var deskDict = dbContext.Desks.Where(d => d.DeskName.StartsWith("AP") && new[] { "Raum 110", "Raum 113" }.Contains(d.Room.RoomName)).ToDictionary(d => d.DeskName + d.Room.RoomName, d => d.DeskId);

                    var assignments = new List<LabelAssignment>
                    {
                        new LabelAssignment { LabelId = labelDict["Eco Friendly"], EntityType = "Room", EntityId = roomDict["Raum 114"] },
                        new LabelAssignment { LabelId = labelDict["VIP"], EntityType = "Room", EntityId = roomDict["Raum 113"] },
                        new LabelAssignment { LabelId = labelDict["Reserved"], EntityType = "Desk", EntityId = deskDict["AP01Raum 110"] },
                        new LabelAssignment { LabelId = labelDict["Public"], EntityType = "Desk", EntityId = deskDict["AP02Raum 113"] }
                    };

                    dbContext.LabelAssignments.AddRange(assignments);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}