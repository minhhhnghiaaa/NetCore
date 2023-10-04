using AutoMapper;
using NetCore.Api.Dtos.Requests.Merchant;

namespace NetCore.Api.MappingProfiles.Merchant;

public class RequestToDomain : Profile
{
    public RequestToDomain()
    {
        CreateMap<CreateMerchantRequestDtos, Domain.Entities.Merchant>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => 1));
        
        CreateMap<UpdateMerchantRequestDtos, Domain.Entities.Merchant>();

        CreateMap<Domain.Entities.Merchant, Domain.Entities.Merchant>();
    }
}