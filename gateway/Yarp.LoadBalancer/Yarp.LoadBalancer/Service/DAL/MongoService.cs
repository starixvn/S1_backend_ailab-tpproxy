using MongoDB.Driver;
using Yarp.LoadBalancer.Service.Model;

namespace Yarp.LoadBalancer.Service.DAL
{
    public interface ITpAuthTokenService
    {
        TpAuthToken GetTpAuthToken(string token);
    }

    public class TpAuthTokenService : ITpAuthTokenService
    {
        private readonly IMongoCollection<TpAuthToken> _collection;

        public TpAuthTokenService(IMongoClient mongoClient)
        {
            // Todo 抽到SiteConfig
            var database = mongoClient.GetDatabase("TpAuthDB");
            _collection = database.GetCollection<TpAuthToken>("TpAuthToken");
        }

        public TpAuthToken GetTpAuthToken(string token)
        {
            var filter = Builders<TpAuthToken>.Filter.Eq(doc => doc.Token, token);
            return _collection.Find(filter).FirstOrDefault();
        }
    }
}
