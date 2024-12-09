using AutoMapper;
using CoworkingSpaceAPI.Dtos.Auth.Response;
using CoworkingSpaceAPI.Models;

namespace CoworkingSpaceAPI.Mapping
{
    public class UserProfileMappingProfile : Profile
    {
        public UserProfileMappingProfile()
        {
            CreateMap<ApplicationUserModel, UserDetailsDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles will be set separately
            CreateMap<Address, UserDetailsDto>();
        }
    }
}