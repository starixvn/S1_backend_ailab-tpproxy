using System.Reflection;
using MongoDB.Driver;
using Yarp.LoadBalancer.Service.Model;

namespace Yarp.LoadBalancer.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMultipleImplementingInterface<TInterface>(this IServiceCollection services)
        {
            var interfaceType = typeof(TInterface);
            var implementingTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.GetInterfaces().Contains(interfaceType));

            foreach (var implementingType in implementingTypes)
            {
                services.AddTransient(interfaceType, implementingType);
            }

            return services;
        }

        public static IServiceCollection AddMonGoDbClient(this IServiceCollection services)
        {
            //todo 根據不同環境取得連線字串
            services.AddSingleton<IMongoClient>(sp => new MongoClient("mongodb://admin:admin@localhost:27017")); 
            return services;
        }

        public static IServiceCollection AddClusterSettingConfig(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json") 
                .Build();

            // 集群對應設定
            var clusterNameMappings = configuration.GetSection("ClusterNameMappings").Get<Dictionary<string, string>>();
            services.AddSingleton(clusterNameMappings);

            return services;
        }

        public static IServiceCollection AddMonGoDbSetting(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var monGoDbSetting = configuration.GetSection("MonGoDbSetting").Get<MongoDbSetting>();
            services.AddSingleton(monGoDbSetting);
            return services;
        }
    }
}
