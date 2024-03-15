using AuctionLeague.Data;

namespace AuctionLeague.MongoDb.Abstractions;

public interface IPlayerRepository
{
    Task<IEnumerable<Player>> GetPlayersAsync();
    Task<Player> GetPlayerAsync(int playerId);
    Task AddPlayerAsync(Player newPlayer);
    Task AddPlayersAsync(IEnumerable<Player> newPlayer);
    Task UpdatePlayerAsync(int playerId, Player updatedPlayer);
    Task RemovePlayerAsync(int playerId);
    Task RemoveAllFplPlayersAsync();
}