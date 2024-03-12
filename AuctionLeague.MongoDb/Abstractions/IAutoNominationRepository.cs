using AuctionLeague.Data;

namespace AuctionLeague.MongoDb.Abstractions
{
    public interface IAutoNominationRepository
    {
        Task<List<Player>> GetAutoNominationsAsync();
        
        Task<List<Player>> GetAutoNominationsForPositionAsync(Position position);
        
        Task SaveAutoNominationDataAsync(List<Player> data);

        Task RemoveAutonominationDataAsync();
    }
}