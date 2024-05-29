using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.MongoDb.Abstractions;

public interface IManualPlayerRepository
{
    Task<IEnumerable<ManualPlayer>> GetPlayersAsync();
    Task<ManualPlayer> GetPlayerAsync(int playerId);
    Task<IEnumerable<ManualPlayer>> GetPlayerAsync(string lastNameSearch);
    Task AddPlayerAsync(ManualPlayer newPlayer);
    Task AddPlayersAsync(IEnumerable<ManualPlayer> newPlayer);
    Task UpdatePlayerAsync(int playerId, ManualPlayer updatedPlayer);
    Task RemovePlayerAsync(int playerId);
    Task RemoveAllPlayersAsync();
}