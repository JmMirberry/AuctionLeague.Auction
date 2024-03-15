using AuctionLeague.Data;

namespace AuctionLeague.MongoDb.Abstractions
{
    public interface IAutoNominationRepository
    {
        Task<IEnumerable<Player>> GetAutoNominationsAsync();
        
        Task<IEnumerable<Player>> GetAutoNominationsForPositionAsync(Position position);
        
        Task SaveAutoNominationDataAsync(IEnumerable<Player> data);

        Task RemoveAutonominationDataAsync();
    }
}