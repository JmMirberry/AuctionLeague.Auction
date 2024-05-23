using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class ManualPlayerRepository : BaseRepository, IManualPlayerRepository
    {
        private readonly IMongoCollection<ManualPlayerEntity> _playersCollection;

        public ManualPlayerRepository(
            IOptions<MongoDbSettings> settings) : base(settings) 
        {
            _playersCollection = mongoDatabase.GetCollection<ManualPlayerEntity>("FplPlayers");
        }

        public async Task<IEnumerable<ManualPlayer>> GetPlayersAsync()
        {
            var entities =
            await _playersCollection.Find(_ => true).ToListAsync();
            return entities?.Select(e => e.ToPlayer());
        }

        public async Task<ManualPlayer> GetPlayerAsync(int playerId)
        {
            var entity =
            await _playersCollection.Find(x => x.PlayerId == playerId).FirstOrDefaultAsync();
            return entity?.ToPlayer();
        }

        public async Task<IEnumerable<ManualPlayer>> GetPlayerAsync(string lastNameSearch)
        {
            var entities =
            await _playersCollection.Find(x => x.LastName.Contains(lastNameSearch)).ToListAsync();
            return entities?.Select(e => e.ToPlayer());
        }

        public async Task AddPlayerAsync(ManualPlayer newPlayer) =>
          await _playersCollection.InsertOneAsync(newPlayer.ToEntity());

        public async Task AddPlayersAsync(IEnumerable<ManualPlayer> newPlayer) =>
            await _playersCollection.InsertManyAsync(newPlayer.Select(p=>p.ToEntity()));

        public async Task UpdatePlayerAsync(int playerId, ManualPlayer updatedPlayer) =>
           await _playersCollection.ReplaceOneAsync(x => x.PlayerId == playerId, updatedPlayer.ToEntity());

        public async Task RemovePlayerAsync(int playerId) =>
            await _playersCollection.DeleteOneAsync(x => x.PlayerId == playerId);

        public async Task RemoveAllPlayersAsync() =>
            await _playersCollection.DeleteManyAsync(_ => true);
    }
}