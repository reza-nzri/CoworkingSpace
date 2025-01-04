using AutoMapper;
using CoworkingSpaceAPI.Dtos.Address.Request;
using CoworkingSpaceAPI.Dtos.Auth.Request;
using CoworkingSpaceAPI.Dtos.Auth.Response;
using CoworkingSpaceAPI.Dtos.CEO.CoworkingSpaceAPI.Dtos.Company.Response;
using CoworkingSpaceAPI.Dtos.Company.Request;
using CoworkingSpaceAPI.Dtos.Company.Response;
using CoworkingSpaceAPI.Dtos.Desk.Request;
using CoworkingSpaceAPI.Dtos.Desk.Response;
using CoworkingSpaceAPI.Dtos.Room;
using CoworkingSpaceAPI.Dtos.Room.Response;
using CoworkingSpaceAPI.Models;

namespace CoworkingSpaceAPI.Mapping
{
    public class UserProfileMappingProfile : Profile
    {
        public UserProfileMappingProfile()
        {
            // Mapping from ApplicationUserModel to UserDetailsDto
            CreateMap<ApplicationUserModel, UserDetailsDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles will be set separately

            // Mapping from Address to UserDetailsDto
            CreateMap<Address, UserDetailsDto>();

            // Mapping from UserDetailsDto to Address
            CreateMap<UserDetailsDto, Address>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null values

            // Mapping from UserDetailsDto to UserAddress
            CreateMap<UserDetailsDto, UserAddress>()
                .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src)) // Map Address object
                .ForMember(dest => dest.AddressType, opts => opts.MapFrom(src => new AddressType { AddressTypeName = src.AddressType })); // Map AddressType explicitly

            // Mapping from UserDetailsDto to ApplicationUserModel
            CreateMap<UserDetailsDto, ApplicationUserModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null values

            // Mapping from ApplicationUserModel to UserDetailsUpdatedDto
            CreateMap<ApplicationUserModel, UserDetailsUpdatedDto>();

            // Mapping from Address to UserDetailsUpdatedDto
            CreateMap<Address, UserDetailsUpdatedDto>();

            // Mapping from UserDetailsUpdatedDto to Address
            CreateMap<UserDetailsUpdatedDto, Address>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null values

            // Mapping from UserDetailsUpdatedDto to UserAddress
            CreateMap<UserDetailsUpdatedDto, UserAddress>()
                .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src)) // Map Address object
                .ForMember(dest => dest.AddressType, opts => opts.MapFrom(src => new AddressType { AddressTypeName = src.AddressType })); // Map AddressType explicitly

            // Mapping from UserDetailsUpdatedDto to ApplicationUserModel
            CreateMap<UserDetailsUpdatedDto, ApplicationUserModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null values

            CreateMap<Company, ResponseCompanyDto>();
            CreateMap<Address, AddressDto>();

            CreateMap<RegisterCompanyReqDto, Company>()
                .ForMember(dest => dest.FoundedDate, opt => opt.MapFrom(src => src.FoundedDate ?? DateOnly.FromDateTime(DateTime.UtcNow)));

            CreateMap<CompanyCeo, CompanyCeoDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.CeoUser.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.CeoUser.LastName))
                .ForMember(dest => dest.Instagram, opt => opt.MapFrom(src => src.CeoUser.Instagram));

            CreateMap<Company, CompanyDetailsDto>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.CompanyAddresses.FirstOrDefault().Address.Street))
                .ForMember(dest => dest.HouseNumber, opt => opt.MapFrom(src => src.CompanyAddresses.FirstOrDefault().Address.HouseNumber))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.CompanyAddresses.FirstOrDefault().Address.PostalCode))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.CompanyAddresses.FirstOrDefault().Address.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.CompanyAddresses.FirstOrDefault().Address.State))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CompanyAddresses.FirstOrDefault().Address.Country))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.CompanyAddresses.FirstOrDefault().AddressType.AddressTypeName))
                .ForMember(dest => dest.TypeDescription, opt => opt.MapFrom(src => src.CompanyAddresses.FirstOrDefault().AddressType.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CompanyCeos.FirstOrDefault().StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.CompanyCeos.FirstOrDefault().EndDate));

            CreateMap<CreateRoomDto, Room>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyAddressId, opt => opt.Ignore());

            CreateMap<AddRoomDto, Room>()
                 .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.RoomName))
                 .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType))
                 .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                 .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                 .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))  // Default to true
                 .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())  // Set manually
                 .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<AddAddressDto, Address>();

            CreateMap<AddEmployeeDto, CompanyEmployee>()
               .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
               .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
               .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));

            CreateMap<CompanyEmployee, EmployeeDetailsDto>()
                .ForMember(dest => dest.EmployeeUsername, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));

            CreateMap<DeleteEmployeeRequestDto, CompanyEmployee>();

            CreateMap<UpdateCompanyDetailsDto, Company>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null && (srcMember is not string || !string.IsNullOrWhiteSpace(srcMember.ToString()))));

            CreateMap<Room, RoomDetailsDto>();

            CreateMap<AddDeskDto, Desk>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))  // Set CreatedAt to now
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency ?? "EUR"))  // Default currency
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable ?? true));  // Default availability

            CreateMap<Desk, DeskDetailsDto>();

            CreateMap<Desk, DeskDetailsWithRoomDto>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId));

            CreateMap<Desk, DeskDetailsWithRoomDto>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId));

            CreateMap<Desk, DeskDetailsWithRoomDto>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId));

            CreateMap<Desk, DeskDetailsWithRoomDto>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId));

            CreateMap<UpdateDeskDto, Desk>()
                .ForMember(dest => dest.DeskName, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.DeskName)))
                .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price.HasValue))
                .ForMember(dest => dest.Currency, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Currency)))
                .ForMember(dest => dest.IsAvailable, opt => opt.Condition(src => src.IsAvailable.HasValue));

            CreateMap<Room, RoomDetailsDto>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.RoomName))
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price ?? 0))  // Handle null price
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency ?? "EUR"))  // Default if null
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}