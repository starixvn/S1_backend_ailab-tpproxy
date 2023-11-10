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

        public TpAuthTokenService(IMongoClient client, MongoDbSetting dbSetting)
        {
            var database = client.GetDatabase(dbSetting.DbName);
            _collection = database.GetCollection<TpAuthToken>(dbSetting.CollectionName);
        }

        public TpAuthToken GetTpAuthToken(string token)
        {
            var filter = Builders<TpAuthToken>.Filter.Eq(doc => doc.Token, token);
            return _collection.Find(filter).FirstOrDefault();
        }
    }
}
