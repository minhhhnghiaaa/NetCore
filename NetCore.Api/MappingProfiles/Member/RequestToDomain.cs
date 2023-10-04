using AutoMapper;
using NetCore.Api.Dtos.Requests.Member;

namespace NetCore.Api.MappingProfiles.Member;

public class RequestToDomain : Profile
{
    public RequestToDomain()
    {
        CreateMap<CreateMerchantMemberRequestDtos, Domain.Entities.Member>()
            .ForMember(dest => dest.Phone,
                opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => 1));

        CreateMap<UpdateMerchantMemberRequestDtos, Domain.Entities.Member>()
            .ForMember(dest => dest.Phone,
                opt => opt.MapFrom(src => src.PhoneNumber));

        CreateMap<Domain.Entities.Member, Domain.Entities.Member>();
    }
}