using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDb.Entities;

namespace AuctionLeague.MongoDb.Repositories
{
    public class PlayerRepository : BaseRepository
    {
        private readonly IMongoCollection<PlayerEntity> _playersCollection;

        public PlayerRepository(
            IOptions<MongoDbSettings> settings) : base(settings) 
        {
            
            _playersCollection = mongoDatabase.GetCollection<PlayerEntity>(
                "Players");
        }

        public async Task<List<PlayerEntity>> GetAsync() =>
            await _playersCollection.Find(_ => true).ToListAsync();

        public async Task<PlayerEntity> GetAsync(int playerId) =>
            await _playersCollection.Find(x => x.PlayerId == playerId).FirstOrDefaultAsync();

        public async Task CreateAsync(PlayerEntity newPlayer) =>
            await _playersCollection.InsertOneAsync(newPlayer);

        public async Task UpdateAsync(int playerId, PlayerEntity updatedPlayer) =>
            await _playersCollection.ReplaceOneAsync(x => x.PlayerId == playerId, updatedPlayer);

        public async Task RemoveAsync(int playerId) =>
            await _playersCollection.DeleteOneAsync(x => x.PlayerId == playerId);
    }
}