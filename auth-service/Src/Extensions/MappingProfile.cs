using auth_service.DTOs.Auth;
using auth_service.DTOs.Token;
using auth_service.Models;
using AutoMapper;

namespace auth_service.Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, LoginResponseDto>();
        
        CreateMap<TokenRevokeRequest, TokenBlackList>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<TokenRevokeRequest, TokenRevokeResponse>()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));
    }
    
}