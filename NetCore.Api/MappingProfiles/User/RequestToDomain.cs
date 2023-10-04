using AutoMapper;
using NetCore.Api.Dtos.Requests.User;

namespace NetCore.Api.MappingProfiles.User;

public class RequestToDomain : Profile
{
    public RequestToDomain()
    {
        CreateMap<CreateUserRequestDtos, Domain.Entities.User>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.CreatedDate,
                opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedDate,
                opt => opt.MapFrom(src => DateTime.UtcNow))
            ;
    }
}