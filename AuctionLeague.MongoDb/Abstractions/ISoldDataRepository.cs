using AuctionLeague.Data;

namespace AuctionLeague.MongoDb.Abstractions
{
    public interface ISoldDataRepository
    {
        Task<IEnumerable<SoldData>> GetSoldDataAsync();

        Task<SoldData> GetSoldDataAsync(int playerId);

        Task AddSoldDataAsync(SoldData newSoldData);
        

        Task RemoveSoldDataAsync(int playerId);

        Task RemoveAllSoldDataAsync();
    }
}