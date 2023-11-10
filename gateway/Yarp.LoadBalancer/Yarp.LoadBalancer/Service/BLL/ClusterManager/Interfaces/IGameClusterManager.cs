using Yarp.LoadBalancer.Service.Enums;

namespace Yarp.LoadBalancer.Service.BLL.ClusterManager.Interfaces
{
    public interface IGameClusterManager
    {
        public string TpName { get; set; }

        public Task<ClusterName> SelectorCluster(HttpRequest request);
    }
}
