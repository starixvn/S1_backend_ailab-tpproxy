using System.Text.RegularExpressions;
using Yarp.LoadBalancer.Service.BLL.ClusterManager.Interfaces;

namespace Yarp.LoadBalancer.Extensions.Middlewares
{
    public class CustomRoutingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<IGameClusterManager> _gameClusterManagers;
        private const string RouteId = "TpRoute";

        public CustomRoutingMiddleware(RequestDelegate next, IEnumerable<IGameClusterManager> gameClusterManagers)
        {
            _next = next;
            _gameClusterManagers = gameClusterManagers;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var tpName = GetTpName(context.Request.Path);
                var gameClusterManager = _gameClusterManagers.First(r => r.TpName.Equals(tpName, StringComparison.OrdinalIgnoreCase));
                var clusterId = await gameClusterManager.SelectorCluster(context.Request);

                // 設置集群Id 與 路由 Id
                context.Items["ReverseProxy:SelectedCluster"] = clusterId;
                context.Items["ReverseProxy:SelectedRoute"] = RouteId;
            }
            catch (Exception ex)
            {
                //to error log
                Console.WriteLine(ex);
            }

            await _next(context);
        }

        private static string GetTpName(string path)
        {
            // 使用正则表达式匹配
            var tpName = string.Empty;
            const string pattern = @"/(.*?)/";
            var match = Regex.Match(path, pattern);
            if (match.Success)
            {
                tpName = match.Groups[1].Value;
            }

            return tpName;
        }
    }
}
