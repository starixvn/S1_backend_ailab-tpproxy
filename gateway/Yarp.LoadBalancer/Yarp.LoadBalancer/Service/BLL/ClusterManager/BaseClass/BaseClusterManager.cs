using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml;
using Yarp.LoadBalancer.Service.Enums;

namespace Yarp.LoadBalancer.Service.BLL.ClusterManager.BaseClass
{
    public class BaseClusterManager
    {
        private readonly Dictionary<string, string> _siteNameOfCluster;

        public BaseClusterManager(Dictionary<string, string> siteNameOfCluster)
        {
            _siteNameOfCluster = siteNameOfCluster;
        }

        protected async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            var requestBodyStream = new MemoryStream();
            await request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            string requestBodyContent;
            using (var requestBody = new StreamReader(requestBodyStream, Encoding.UTF8, true, 1024, true))
            {
                requestBodyContent = await requestBody.ReadToEndAsync();
            }

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            request.Body = requestBodyStream;

            return requestBodyContent;
        }

        protected string GetTpUserNameFromJson(string json, string jsonPath)
        {
            var jObject = JObject.Parse(json);
            var userTypeToken = jObject.SelectToken(jsonPath);
            return userTypeToken.Value<string>();
        }

        protected string GetTpUserNameFromXml(string xmlString, string xmlPath)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            var node = xmlDoc.SelectSingleNode(xmlPath);
            return node.InnerText;
        }

        protected string GetActionName(string requestPath)
        {
            var paths = requestPath.Split("/", StringSplitOptions.RemoveEmptyEntries);
            return paths.Length > 1 ? paths[1] : string.Empty;
        }

        protected ClusterName SelectCluster(string tpUserName)
        {
            const ClusterName defaultCluster = ClusterName.nova;

            foreach (var kvp in _siteNameOfCluster.Where(kvp => tpUserName.IndexOf(kvp.Key, StringComparison.OrdinalIgnoreCase) != -1))
            {
                if (Enum.TryParse<ClusterName>(kvp.Value, out var cluster))
                {
                    return cluster;
                }
            }

            return defaultCluster;
        }
    }
}
