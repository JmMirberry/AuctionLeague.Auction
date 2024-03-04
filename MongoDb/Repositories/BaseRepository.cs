using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class BaseRepository
    {
        protected readonly IMongoDatabase mongoDatabase;

        public BaseRepository(
            IOptions<MongoDbSettings> settings)
        {
            var mongoClient = new MongoClient(
                settings.Value.ConnectionString);

            mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);
        }
    }
}