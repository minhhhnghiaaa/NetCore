using AutoMapper;
using NetCore.Api.Dtos.Responses.Member;

namespace NetCore.Api.MappingProfiles.Member;

public class DomainToResponse : Profile
{
    public DomainToResponse()
    {
        CreateMap<Domain.Entities.Member, MerchantMemberResponseDtos>()
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.Phone))
            ;
    }
}