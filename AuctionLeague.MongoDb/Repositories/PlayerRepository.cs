using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.MongoDb.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class PlayerRepository : BaseRepository, IPlayerRepository
    {
        private readonly IMongoCollection<PlayerEntity> _playersCollection;

        public PlayerRepository(
            IOptions<MongoDbSettings> settings) : base(settings) 
        {
            _playersCollection = mongoDatabase.GetCollection<PlayerEntity>("Players");
        }

        public async Task<List<PlayerEntity>> GetPlayersAsync() =>
            await _playersCollection.Find(_ => true).ToListAsync();

        public async Task<PlayerEntity> GetPlayerAsync(int playerId) =>
            await _playersCollection.Find(x => x.PlayerId == playerId).FirstOrDefaultAsync();

        public async Task AddPlayerAsync(PlayerEntity newPlayer) =>
            await _playersCollection.InsertOneAsync(newPlayer);
        
        public async Task AddPlayersAsync(IEnumerable<PlayerEntity> newPlayer) =>
            await _playersCollection.InsertManyAsync(newPlayer);

        public async Task UpdatePlayerAsync(int playerId, PlayerEntity updatedPlayer) =>
            await _playersCollection.ReplaceOneAsync(x => x.PlayerId == playerId, updatedPlayer);

        public async Task RemovePlayerAsync(int playerId) =>
            await _playersCollection.DeleteOneAsync(x => x.PlayerId == playerId);

        public async Task RemoveAllFplPlayersAsync() =>
            await _playersCollection.DeleteManyAsync(p => p.IsInFpl);
    }
}