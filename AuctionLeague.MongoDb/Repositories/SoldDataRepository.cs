using AuctionLeague.Data;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class SoldDataRepository : BaseRepository, ISoldDataRepository
    {
        private readonly IMongoCollection<SoldData> _soldDataCollection;

        public SoldDataRepository(
            IOptions<MongoDbSettings> settings) : base(settings) 
        {
            _soldDataCollection = mongoDatabase.GetCollection<SoldData>("SoldData");
        }

        public async Task<List<SoldData>> GetSoldDataAsync() =>
            await _soldDataCollection.Find(_ => true).ToListAsync();

        public async Task<SoldData> GetSoldDataAsync(int playerId) =>
            await _soldDataCollection.Find(x => x.PlayerId == playerId).FirstOrDefaultAsync();

        public async Task AddSoldDataAsync(SoldData newSoldData) =>
            await _soldDataCollection.InsertOneAsync(newSoldData);
        

        public async Task RemoveSoldDataAsync(int playerId) =>
            await _soldDataCollection.DeleteOneAsync(x => x.PlayerId == playerId);

        public async Task RemoveAllSoldDataAsync() =>
            await _soldDataCollection.DeleteManyAsync(_ => true);
    }
}