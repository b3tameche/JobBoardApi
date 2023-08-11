using AutoMapper;
using update.Models.Domain;
using update.Models.DTOs;

namespace update.Mappings;

public class CustomMappingProfile : Profile
{
    public CustomMappingProfile() {

        CreateMap<UserRegisterDTO, User>()
            .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Email))
            .ReverseMap();
        
        CreateMap<JobDTO, Job>()
            .ForMember(dst => dst.PostDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dst => dst.ExpireDate, opt => opt.MapFrom(src => DateTime.Now.AddDays(14)))
            .ReverseMap();
        
        CreateMap<User, UserGetByIdDTO>();
    }
}