using AuctionLeague.Data;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class AutoNominationRepository : BaseRepository, IAutoNominationRepository
    {
        private readonly IMongoCollection<Player> _autoNominationCollection;

        public AutoNominationRepository(
            IOptions<MongoDbSettings> settings) : base(settings)
        {
            _autoNominationCollection = mongoDatabase.GetCollection<Player>("AutoNomination");
        }

        public async Task<List<Player>> GetAutoNominationsAsync() =>
            await _autoNominationCollection.Find(_ => true).ToListAsync();

        public async Task<List<Player>> GetAutoNominationsForPositionAsync(Position position) =>
            await _autoNominationCollection.Find(x => x.Position == position).ToListAsync();

        public async Task SaveAutoNominationDataAsync(List<Player> data) =>
            await _autoNominationCollection.InsertManyAsync(data);

        public async Task RemoveAutonominationDataAsync() =>
       await _autoNominationCollection.DeleteManyAsync(_ => true);


    }
}