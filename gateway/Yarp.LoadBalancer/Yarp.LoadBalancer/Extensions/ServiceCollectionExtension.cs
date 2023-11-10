using System.Reflection;
using MongoDB.Driver;

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

        public static IServiceCollection AddMonGoDbClient(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(sp => new MongoClient("mongodb://admin:admin@localhost:27017"));
            return services;
        }

    }
}
