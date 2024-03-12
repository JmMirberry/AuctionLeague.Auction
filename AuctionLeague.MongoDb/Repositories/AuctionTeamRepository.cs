using AuctionLeague.Data;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class AuctionTeamRepository : BaseRepository, IAuctionTeamRepository
    {
        private readonly IMongoCollection<AuctionTeam> _auctionTeamsCollection;

        public AuctionTeamRepository(
            IOptions<MongoDbSettings> settings) : base(settings)
        {
            _auctionTeamsCollection = mongoDatabase.GetCollection<AuctionTeam>("AuctionTeams");
        }

        public async Task<List<AuctionTeam>> GetAuctionTeamsAsync() =>
            await _auctionTeamsCollection.Find(_ => true).ToListAsync();

        public async Task<AuctionTeam> GetAuctionTeamAsync(string teamName) =>
            await _auctionTeamsCollection.Find(x => x.TeamName == teamName).FirstOrDefaultAsync();

        public async Task<AuctionTeam> GetAuctionTeamByBidderAsync(string bidder) =>
        await _auctionTeamsCollection.Find(x => x.SlackBidders.Any(x => x == bidder)).FirstOrDefaultAsync();

        public async Task AddAuctionTeamAsync(AuctionTeam newAuctionTeam) =>
            await _auctionTeamsCollection.InsertOneAsync(newAuctionTeam);

        public async Task AddAuctionTeamsAsync(IEnumerable<AuctionTeam> newAuctionTeam) =>
            await _auctionTeamsCollection.InsertManyAsync(newAuctionTeam);

        public async Task UpdateAuctionTeamAsync(AuctionTeam updatedAuctionTeam) =>
            await _auctionTeamsCollection.ReplaceOneAsync(x => x.TeamName == updatedAuctionTeam.TeamName, updatedAuctionTeam);

        public async Task AddPlayerToAuctionTeamAsync(string teamName, SoldPlayer soldPlayer)
        {
            var pushPlayerDefinition = Builders<AuctionTeam>.Update.Push(t => t.Players, soldPlayer);
            await _auctionTeamsCollection.UpdateOneAsync(x => x.TeamName == teamName, pushPlayerDefinition);
        }

        public async Task RemovePlayersFromAuctionTeamAsync(string teamName)
        {
            var players = Builders<AuctionTeam>.Update.PullAll("Players",  new BsonArray());
            await _auctionTeamsCollection.UpdateOneAsync(x => x.TeamName == teamName, players);
        }
        
        public async Task RemovePlayersFromAllAuctionTeams()
        {
            var players = Builders<AuctionTeam>.Update.PullAll("Players",  new BsonArray());
            await _auctionTeamsCollection.UpdateManyAsync(_ => true, players);
        }

        public async Task RemoveAuctionTeamAsync(string teamName) =>
            await _auctionTeamsCollection.DeleteOneAsync(x => x.TeamName == teamName);

        public async Task RemoveAllAuctionTeamsAsync() =>
            await _auctionTeamsCollection.DeleteManyAsync(_ => true);
    }
}