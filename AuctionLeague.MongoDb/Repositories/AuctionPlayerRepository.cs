using AuctionLeague.Data.Auction;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class AuctionPlayerRepository : BaseRepository, IAuctionPlayerRepository
    {
        private readonly IMongoCollection<AuctionPlayerEntity> _playersCollection;

        public AuctionPlayerRepository(
            IOptions<MongoDbSettings> settings) : base(settings) 
        {
            _playersCollection = mongoDatabase.GetCollection<AuctionPlayerEntity>("AuctionPlayers");
        }

        public async Task<IEnumerable<AuctionPlayer>> GetPlayersAsync()
        {
            var entities =
            await _playersCollection.Find(_ => true).ToListAsync();
            return entities?.Select(e => e.ToPlayer());
        }

        public async Task<AuctionPlayer> GetPlayerAsync(int playerId)
        {
            var entity =
            await _playersCollection.Find(x => x.PlayerId == playerId).FirstOrDefaultAsync();
            return entity?.ToPlayer();
        }

        public async Task<IEnumerable<AuctionPlayer>> GetPlayerAsync(string lastNameSearch)
        {
            var entities =
            await _playersCollection.Find(x => x.LastName.Contains(lastNameSearch)).ToListAsync();
            return entities?.Select(e => e.ToPlayer());
        }

        public async Task AddPlayerAsync(AuctionPlayer newPlayer) =>
            await _playersCollection.InsertOneAsync(newPlayer.ToEntity());
        
        public async Task AddPlayersAsync(IEnumerable<AuctionPlayer> newPlayer) =>
            await _playersCollection.InsertManyAsync(newPlayer.Select(p=>p.ToEntity()));

        public async Task UpdatePlayerAsync(int playerId, AuctionPlayer updatedPlayer) =>
            await _playersCollection.ReplaceOneAsync(x => x.PlayerId == playerId, updatedPlayer.ToEntity());

        public async Task RemovePlayerAsync(int playerId) =>
            await _playersCollection.DeleteOneAsync(x => x.PlayerId == playerId);

        public async Task RemoveAllPlayersAsync() =>
            await _playersCollection.DeleteManyAsync(p => p.IsFplPlayer);

        public async Task SetPlayerAsSold(int playerId)
        {
            var filter = Builders<AuctionPlayerEntity>.Filter.Eq(p => p.PlayerId, playerId);
            var update = Builders<AuctionPlayerEntity>.Update.Set(p => p.IsSold, true);
            await _playersCollection.UpdateOneAsync(filter, update);
        }

        public async Task ResetSold()
        {
            var filter = Builders<AuctionPlayerEntity>.Filter.Empty;
            var update = Builders<AuctionPlayerEntity>.Update.Set(p => p.IsSold, false);
            await _playersCollection.UpdateOneAsync(filter, update);
        }
    }
}