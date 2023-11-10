using System.Text;
using System.Text.RegularExpressions;

namespace Yarp.LoadBalancer.Extensions.Middlewares
{
    public class CustomRoutingMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomRoutingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            var originalRequestBody = context.Request.Body;

            var requestBodyStream = new MemoryStream();
            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            string requestBodyContent;
            using (var requestBody = new StreamReader(requestBodyStream, Encoding.UTF8, true, 1024, true))
            {
                requestBodyContent = await requestBody.ReadToEndAsync();
            }

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;


            // todo 根據三方實作，找 TpUserName or Brand
            string userName = ParseUserName(requestBodyContent);

            // todo 根據三方實作
            string clusterId = SelectClusterId(userName);

            // 设置选择的集群和路由规则
            context.Items["ReverseProxy:SelectedCluster"] = clusterId;
            context.Items["ReverseProxy:SelectedRoute"] = $"TpRoute";
            await _next(context);
        }

        private string ParseUserName(string requestBodyContent)
        {
            return "leobet_12345";
        }

        private string SelectClusterId(string requestContent)
        {
            return
              requestContent.Contains("nova") ||
              requestContent.Contains("bwing") ||
              requestContent.Contains("goal") ? "nova" : "glaxy";
        }

        private string GetRoute(string path)
        {
            // 使用正则表达式匹配
            var tpName = string.Empty;
            string pattern = @"/(.*?)/";
            Match match = Regex.Match(path, pattern);
            if (match.Success)
            {
                tpName = match.Groups[1].Value;
            }

            return tpName;
        }
    }
}
