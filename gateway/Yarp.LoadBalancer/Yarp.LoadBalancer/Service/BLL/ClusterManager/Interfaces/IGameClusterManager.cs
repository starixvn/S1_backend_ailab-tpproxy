namespace Yarp.LoadBalancer.Service.BLL.ClusterManager.Interfaces
{
    public interface IGameClusterManager
    {
        public string TpName { get; set; }

        public Task<string> SelectorCluster(HttpRequest request);
    }
}
