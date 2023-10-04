using AutoMapper;
using NetCore.Api.Dtos.Requests.User;
using NetCore.Api.Dtos.Responses.User;

namespace NetCore.Api.MappingProfiles.User;

public class DomainToResponse : Profile
{
    public DomainToResponse()
    {
        CreateMap<Domain.Entities.User, GetUserResponseDtos>()
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.Id))
            ;
        CreateMap<Domain.Entities.User, CreateTokenRequestDtos>()
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.Id))
            ;
    }
}