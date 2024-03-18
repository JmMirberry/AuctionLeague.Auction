using AuctionLeague.Data;
using AuctionLeague.MongoDb.Abstractions;
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

        public async Task<IEnumerable<Player>> GetPlayersAsync()
        {
            var entities =
            await _playersCollection.Find(_ => true).ToListAsync();
            return entities?.Select(e => e.ToPlayer());
        }

        public async Task<Player> GetPlayerAsync(int playerId)
        {
            var entity =
            await _playersCollection.Find(x => x.PlayerId == playerId).FirstOrDefaultAsync();
            return entity?.ToPlayer();
        }

        public async Task<IEnumerable<Player>> GetPlayerAsync(string lastNameSearch)
        {
            var entities =
            await _playersCollection.Find(x => x.LastName.Contains(lastNameSearch)).ToListAsync();
            return entities?.Select(e => e.ToPlayer());
        }

        public async Task AddPlayerAsync(Player newPlayer) =>
            await _playersCollection.InsertOneAsync(newPlayer.ToEntity());
        
        public async Task AddPlayersAsync(IEnumerable<Player> newPlayer) =>
            await _playersCollection.InsertManyAsync(newPlayer.Select(p=>p.ToEntity()));

        public async Task UpdatePlayerAsync(int playerId, Player updatedPlayer) =>
            await _playersCollection.ReplaceOneAsync(x => x.PlayerId == playerId, updatedPlayer.ToEntity());

        public async Task RemovePlayerAsync(int playerId) =>
            await _playersCollection.DeleteOneAsync(x => x.PlayerId == playerId);

        public async Task RemoveAllFplPlayersAsync() =>
            await _playersCollection.DeleteManyAsync(p => p.IsInFpl);
    }
}