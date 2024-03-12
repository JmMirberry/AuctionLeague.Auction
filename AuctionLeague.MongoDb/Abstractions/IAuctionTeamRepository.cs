using AuctionLeague.Data;

namespace AuctionLeague.MongoDb.Abstractions
{
    public interface IAuctionTeamRepository
    {

        Task<List<AuctionTeam>> GetAuctionTeamsAsync();

        Task<AuctionTeam> GetAuctionTeamAsync(string teamName);

        Task<AuctionTeam> GetAuctionTeamByBidderAsync(string bidder);

        Task AddAuctionTeamAsync(AuctionTeam newAuctionTeam);
        
        Task AddAuctionTeamsAsync(IEnumerable<AuctionTeam> newAuctionTeam);

        Task AddPlayerToAuctionTeamAsync(string teamName, SoldPlayer soldPlayer);

        Task UpdateAuctionTeamAsync(AuctionTeam updatedAuctionTeam);

        Task RemovePlayersFromAuctionTeamAsync(string teamName);

        Task RemovePlayersFromAllAuctionTeams();

        Task RemoveAuctionTeamAsync(string teamName);

        Task RemoveAllAuctionTeamsAsync();
    }
}