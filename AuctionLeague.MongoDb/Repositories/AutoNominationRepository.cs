using AuctionLeague.Data;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class AutoNominationRepository : BaseRepository, IAutoNominationRepository
    {
        private readonly IMongoCollection<PlayerEntity> _autoNominationCollection;

        public AutoNominationRepository(
            IOptions<MongoDbSettings> settings) : base(settings)
        {
            _autoNominationCollection = mongoDatabase.GetCollection<PlayerEntity>("AutoNomination");
        }

        public async Task<IEnumerable<Player>> GetAutoNominationsAsync()
        {
            var entitities = await _autoNominationCollection.Find(_ => true).ToListAsync();
            return entitities.Select(p => p.ToPlayer());
        }
            

        public async Task<IEnumerable<Player>> GetAutoNominationsForPositionAsync(Position position)
        {
            var entitities = await _autoNominationCollection.Find(x => x.Position == position).ToListAsync();
            return entitities.Select(p => p.ToPlayer());
        }

        public async Task SaveAutoNominationDataAsync(IEnumerable<Player> data) =>
            await _autoNominationCollection.InsertManyAsync(data.Select(x=>x.ToEntity()));

        public async Task RemoveAutonominationDataAsync() =>
       await _autoNominationCollection.DeleteManyAsync(_ => true);


    }
}