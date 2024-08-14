using AuctionLeague.Data.Auction;

namespace AuctionLeague.MongoDb.Abstractions
{
    public interface IAutoNominationRepository
    {
        Task AddAutoNominationsAsync(IEnumerable<(int round, List<AuctionPlayer> players)> autoNominations);
        Task<IEnumerable<(int round, List<AuctionPlayer> player)>> GetAutoNominationsAsync();
        Task RemoveAllAsync();
        Task SetPlayerAsNominated(int round, int playerId);
    }
}