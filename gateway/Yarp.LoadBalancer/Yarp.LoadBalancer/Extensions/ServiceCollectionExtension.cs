using System.Reflection;
using MongoDB.Driver;
using Yarp.LoadBalancer.Service.Model;

namespace Yarp.LoadBalancer.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 註冊一個介面多個實作類別
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
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

        /// <summary>
        /// DI MonGoClient 物件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonGoDbClient(this IServiceCollection services, IWebHostEnvironment env)
        {
            var configuration = GetEnvironmentConfig(env);
            var connectionString = configuration.GetSection("MonGoDbConnectionString").Get<string>();
            services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
            return services;
        }

        /// <summary>
        /// DI 集群 Mapping
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddClusterSettingConfig(this IServiceCollection services)
        {
            var configuration = GetGlobalConfig();
            var clusterNameMappings = configuration.GetSection("ClusterNameMappings").Get<Dictionary<string, string>>();
            services.AddSingleton(clusterNameMappings);
            return services;
        }

        /// <summary>
        /// DI MonGoDb 資料庫名稱、集合名稱
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonGoDbSetting(this IServiceCollection services)
        {
            var configuration = GetGlobalConfig();
            var monGoDbSetting = configuration.GetSection("MonGoDbSetting").Get<MongoDbSetting>();
            services.AddSingleton(monGoDbSetting);
            return services;
        }

        #region private method
        
        /// <summary>
        /// 取得全域設定
        /// </summary>
        /// <returns></returns>
        private static IConfigurationRoot GetGlobalConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        /// <summary>
        /// 取得環境設定
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        private static IConfigurationRoot GetEnvironmentConfig(IWebHostEnvironment env)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .Build();
        }

        #endregion
    }
}
