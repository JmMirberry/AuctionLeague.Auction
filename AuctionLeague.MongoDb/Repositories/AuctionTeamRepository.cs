using AuctionLeague.Data.Auction;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AuctionLeague.MongoDb.Repositories
{
    public class AuctionTeamRepository : BaseRepository, IAuctionTeamRepository
    {
        private readonly IMongoCollection<AuctionTeamEntity> _auctionTeamsCollection;

        public AuctionTeamRepository(
            IOptions<MongoDbSettings> settings) : base(settings)
        {
            _auctionTeamsCollection = mongoDatabase.GetCollection<AuctionTeamEntity>("AuctionTeams");
        }

        public async Task<IEnumerable<AuctionTeam>> GetAuctionTeamsAsync()
        {
            var entities = await _auctionTeamsCollection.Find(_ => true).ToListAsync();
            return entities?.Select(t => t.ToTeam());
        }

        public async Task<AuctionTeam> GetAuctionTeamAsync(string teamName)
        {
            var entity = await _auctionTeamsCollection.Find(x => x.TeamName == teamName).FirstOrDefaultAsync();
            return entity?.ToTeam();
        }


        public async Task<AuctionTeam> GetAuctionTeamByBidderAsync(string bidder)
        {
            var entity = await _auctionTeamsCollection.Find(x => x.SlackBidders.Any(x => x == bidder)).FirstOrDefaultAsync();
            return entity?.ToTeam();
        }
        public async Task AddAuctionTeamAsync(AuctionTeam newAuctionTeam) =>
            await _auctionTeamsCollection.InsertOneAsync(newAuctionTeam.ToEntity());

        public async Task AddAuctionTeamsAsync(IEnumerable<AuctionTeam> newAuctionTeam) =>
            await _auctionTeamsCollection.InsertManyAsync(newAuctionTeam.Select(t => t.ToEntity()));

        public async Task UpdateAuctionTeamAsync(AuctionTeam updatedAuctionTeam) =>
            await _auctionTeamsCollection.ReplaceOneAsync(x => x.TeamName == updatedAuctionTeam.TeamName, updatedAuctionTeam.ToEntity());

        public async Task AddPlayerToAuctionTeamAsync(string teamName, SoldPlayer soldPlayer)
        {
            var pushPlayerDefinition = Builders<AuctionTeamEntity>.Update.Push(t => t.Players, soldPlayer.ToSoldPlayerEntity());
            await _auctionTeamsCollection.UpdateOneAsync(x => x.TeamName == teamName, pushPlayerDefinition);
        }

        public async Task RemovePlayersFromAuctionTeamAsync(string teamName)
        {
            var players = Builders<AuctionTeamEntity>.Update.PullAll("Players",  new BsonArray());
            await _auctionTeamsCollection.UpdateOneAsync(x => x.TeamName == teamName, players);
        }
        
        public async Task RemovePlayersFromAllAuctionTeams()
        {
            var players = Builders<AuctionTeamEntity>.Update.PullAll("Players",  new BsonArray());
            await _auctionTeamsCollection.UpdateManyAsync(_ => true, players);
        }

        public async Task RemoveAuctionTeamAsync(string teamName) =>
            await _auctionTeamsCollection.DeleteOneAsync(x => x.TeamName == teamName);

        public async Task RemoveAllAuctionTeamsAsync() =>
            await _auctionTeamsCollection.DeleteManyAsync(_ => true);
    }
}