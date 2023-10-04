using System.Reflection;
using AutoMapper;

namespace NetCore.Api.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        IEnumerable<Type> types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (Type type in types)
        {
            if (type != null)
            {
                MethodInfo? method = type.GetMethod("Mapping") ??
                                     type.GetInterface("IMapFrom`1")!
                                         .GetMethod("Mapping");

                object instance = Activator.CreateInstance(type)!;

                _ = method?.Invoke(instance, new object[] { this });
            }
        }
    }
}