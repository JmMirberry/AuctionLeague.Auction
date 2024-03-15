using AuctionLeague.Data;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class SoldDataRepository : BaseRepository, ISoldDataRepository
    {
        private readonly IMongoCollection<SoldDataEntity> _soldDataCollection;

        public SoldDataRepository(
            IOptions<MongoDbSettings> settings) : base(settings) 
        {
            _soldDataCollection = mongoDatabase.GetCollection<SoldDataEntity>("SoldData");
        }

        public async Task<IEnumerable<SoldData>> GetSoldDataAsync()
        {
            var entities =
            await _soldDataCollection.Find(_ => true).ToListAsync();
            return entities.Select(x => x.ToSoldData());
        }

        public async Task<SoldData> GetSoldDataAsync(int playerId)
        {
            var entity = await _soldDataCollection.Find(x => x.PlayerId == playerId).FirstOrDefaultAsync();
            return entity.ToSoldData();
        }
        public async Task AddSoldDataAsync(SoldData newSoldData) =>
            await _soldDataCollection.InsertOneAsync(newSoldData.ToEntity());
        

        public async Task RemoveSoldDataAsync(int playerId) =>
            await _soldDataCollection.DeleteOneAsync(x => x.PlayerId == playerId);

        public async Task RemoveAllSoldDataAsync() =>
            await _soldDataCollection.DeleteManyAsync(_ => true);
    }
}