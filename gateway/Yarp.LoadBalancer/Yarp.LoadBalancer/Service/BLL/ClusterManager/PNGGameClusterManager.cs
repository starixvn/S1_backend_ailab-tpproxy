using Yarp.LoadBalancer.Service.BLL.ClusterManager.BaseClass;
using Yarp.LoadBalancer.Service.BLL.ClusterManager.Interfaces;
using Yarp.LoadBalancer.Service.DAL;

namespace Yarp.LoadBalancer.Service.BLL.ClusterManager
{
    public class PNGGameClusterManager : BaseClusterManager, IGameClusterManager
    {
        private readonly ITpAuthTokenService _tpAuthTokenService;
        public PNGGameClusterManager(ITpAuthTokenService tpAuthTokenService)
        {
            _tpAuthTokenService = tpAuthTokenService;
        }

        public string TpName { get; set; } = "PNG";

        public async Task<string> SelectorCluster(HttpRequest request)
        {
            var requestBodyContent = await base.ReadRequestBody(request);
            var actionName = GetActionName(request.Path);
            string tpUserName;

            if (actionName.ToLower() == "authenticate")
            {
                var authToken = GetTpUserNameFromXml(requestBodyContent, "/authenticate/username");
                var tpCustInfo = _tpAuthTokenService.GetTpAuthToken(authToken).Value;
                tpUserName = this.GetTpUserNameFromJson(tpCustInfo, "$.TpUserName");
                return base.SelectCluster(tpUserName);
            }

            var xmlPath = $"/{actionName.ToLower()}/externalId";
            tpUserName = GetTpUserNameFromXml(requestBodyContent, xmlPath);
            return base.SelectCluster(tpUserName);
        }
    }
}
