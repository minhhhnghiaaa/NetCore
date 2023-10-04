using AutoMapper;
using NetCore.Api.Dtos.Responses.Merchant;

namespace NetCore.Api.MappingProfiles.Merchant;

public class DomainToResponse : Profile
{
    public DomainToResponse()
    {
        CreateMap<Domain.Entities.Merchant, GetMerchantResponseDtos>()
            .ForMember(dest => dest.MerchantId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.BankName} {src.Name}"))
            ;

    }
}