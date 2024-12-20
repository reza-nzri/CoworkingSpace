using AutoMapper;
using CoworkingSpaceAPI.Dtos.Address.Request;
using CoworkingSpaceAPI.Dtos.Auth.Request;
using CoworkingSpaceAPI.Dtos.Auth.Response;
using CoworkingSpaceAPI.Dtos.CEO.CoworkingSpaceAPI.Dtos.Company.Response;
using CoworkingSpaceAPI.Dtos.Company.Request;
using CoworkingSpaceAPI.Dtos.Company.Response;
using CoworkingSpaceAPI.Dtos.Room;
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
                .ForMember(dest => dest.CeoUsername, opt => opt.MapFrom(src => src.CompanyCeos.FirstOrDefault().CeoUser.UserName ?? "Unknown"));

            CreateMap<CreateRoomDto, Room>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyAddressId, opt => opt.Ignore());

            CreateMap<AddAddressDto, Address>();
        }
    }
}