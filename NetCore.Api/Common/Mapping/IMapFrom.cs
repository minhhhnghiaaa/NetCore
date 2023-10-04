using AutoMapper;

namespace NetCore.Api.Common.Mapping;

public class IMapFrom<T> where T : class
{
    void Mapping(Profile profile)
    {
        _ = profile.CreateMap(typeof(T), GetType());
    }
}