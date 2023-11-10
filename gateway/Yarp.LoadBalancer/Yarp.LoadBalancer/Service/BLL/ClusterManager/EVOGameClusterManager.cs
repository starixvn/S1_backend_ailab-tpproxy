using Yarp.LoadBalancer.Service.BLL.ClusterManager.BaseClass;
using Yarp.LoadBalancer.Service.BLL.ClusterManager.Interfaces;

namespace Yarp.LoadBalancer.Service.BLL.ClusterManager
{
    public class EvoGameClusterManager : BaseClusterManager, IGameClusterManager
    {
        public string TpName { get; set; } = "EVO";

        public async Task<string> SelectorCluster(HttpRequest request)
        {
            var requestBodyContent = await base.ReadRequestBody(request);
            var tpUserNamePerFix = base.GetTpUserNameFromJson(requestBodyContent, "$.userId");
            return base.SelectCluster(tpUserNamePerFix);
        }
    }
}
