using AuctionLeague.Data.Auction;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class AutoNominationRepository : BaseRepository, IAutoNominationRepository
    {
        private readonly IMongoCollection<AutoNominationEntity> _collection;

        public AutoNominationRepository(
            IOptions<MongoDbSettings> settings) : base(settings)
        {
            _collection = mongoDatabase.GetCollection<AutoNominationEntity>("AutoNominations");
        }

        public async Task<IEnumerable<(int round, List<AuctionPlayer> player)>> GetAutoNominationsAsync()
        {
            var entities =
            await _collection.Find(_ => true).ToListAsync();
            return entities?.Select(e => (e.Round, e.Players.Where(p => !p.Nominated).Select(x => x.ToPlayer()).ToList()));
        }

        public async Task AddAutoNominationsAsync(IEnumerable<(int round, List<AuctionPlayer> players)> autoNominations) =>
            await _collection.InsertManyAsync(autoNominations.Select(e => new AutoNominationEntity
            {
                _id = e.round
                Round = e.round,
                Players = e.players.Select(x => x.ToAutoNominatedEntity()).ToList()
            }));

        public async Task RemoveAllAsync() =>
            await _collection.DeleteManyAsync(p => true);

        public async Task SetPlayerAsNominated(int round, int playerId)
        {
            var filter = Builders<AutoNominationEntity>.Filter.And(
                Builders<AutoNominationEntity>.Filter.Eq(p => p.Round, round),
                Builders<AutoNominationEntity>.Filter.Eq("Players.PlayerId", playerId)
                );

            var update = Builders<AutoNominationEntity>.Update.Set(p => p.Players[-1].Nominated, true);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
