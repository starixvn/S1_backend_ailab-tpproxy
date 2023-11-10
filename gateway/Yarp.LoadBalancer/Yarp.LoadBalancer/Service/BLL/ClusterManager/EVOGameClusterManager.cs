using Yarp.LoadBalancer.Service.BLL.ClusterManager.BaseClass;
using Yarp.LoadBalancer.Service.BLL.ClusterManager.Interfaces;
using Yarp.LoadBalancer.Service.Enums;

namespace Yarp.LoadBalancer.Service.BLL.ClusterManager
{
    public class EVOGameClusterManager : BaseClusterManager, IGameClusterManager
    {
        public EVOGameClusterManager(Dictionary<string, string> siteNameOfCluster) : base(siteNameOfCluster){ }

        public string TpName { get; set; } = "EVO";

        public async Task<ClusterName> SelectorCluster(HttpRequest request)
        {
            var requestBodyContent = await base.ReadRequestBody(request);
            var tpUserName = base.GetTpUserNameFromJson(requestBodyContent, "$.userId");
            return base.SelectCluster(tpUserName);
        }
    }
}
