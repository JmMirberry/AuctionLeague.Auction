using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.MongoDb.Abstractions;

public interface IFplPlayerRepository
{
    Task<IEnumerable<Player>> GetPlayersAsync();
    Task<Player> GetPlayerAsync(int playerId);
    Task<IEnumerable<Player>> GetPlayerAsync(string lastNameSearch);
    Task AddPlayersAsync(IEnumerable<Player> newPlayer);
    Task RemoveAllPlayersAsync();
}