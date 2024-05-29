using AuctionLeague.Data.FplPlayer;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class FplPlayerRepository : BaseRepository, IFplPlayerRepository
    {
        private readonly IMongoCollection<PlayerEntity> _playersCollection;

        public FplPlayerRepository(
            IOptions<MongoDbSettings> settings) : base(settings) 
        {
            _playersCollection = mongoDatabase.GetCollection<PlayerEntity>("FplPlayers");
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
        
        public async Task AddPlayersAsync(IEnumerable<Player> newPlayer) =>
            await _playersCollection.InsertManyAsync(newPlayer.Select(p=>p.ToEntity()));

        public async Task RemoveAllPlayersAsync() =>
            await _playersCollection.DeleteManyAsync(_ => true);
    }
}