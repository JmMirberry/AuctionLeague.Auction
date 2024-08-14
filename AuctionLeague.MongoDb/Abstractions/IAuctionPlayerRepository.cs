using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.MongoDb.Abstractions;

public interface IAuctionPlayerRepository
{
    Task<IEnumerable<AuctionPlayer>> GetPlayersAsync();
    Task<AuctionPlayer> GetPlayerAsync(int playerId);
    Task<IEnumerable<AuctionPlayer>> GetPlayerAsync(string lastNameSearch);
    Task AddPlayerAsync(AuctionPlayer newPlayer);
    Task AddPlayersAsync(IEnumerable<AuctionPlayer> newPlayer);
    Task UpdatePlayerAsync(int playerId, AuctionPlayer updatedPlayer);
    Task RemovePlayerAsync(int playerId);
    Task RemoveAllPlayersAsync();
    Task SetPlayerAsSold(int playerId);
    Task ResetSold();
}