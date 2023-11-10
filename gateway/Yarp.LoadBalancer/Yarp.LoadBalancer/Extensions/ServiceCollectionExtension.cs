using System.Reflection;

namespace Yarp.LoadBalancer.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMultipleImplementingInterface(this IServiceCollection services, Type interfaceType, Assembly assembly)
        {
            var implementingTypes = assembly.GetTypes()
                .Where(type => type.GetInterfaces().Contains(interfaceType));

            foreach (var implementingType in implementingTypes)
            {
                services.AddTransient(interfaceType, implementingType);
            }

            return services;
        }
    }
}
