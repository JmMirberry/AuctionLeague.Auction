using AuctionLeague.MongoDb.Entities;

namespace AuctionLeague.MongoDb.Abstractions;

public interface IPlayerRepository
{
    Task<List<PlayerEntity>> GetPlayersAsync();
    Task<PlayerEntity> GetPlayerAsync(int playerId);
    Task AddPlayerAsync(PlayerEntity newPlayer);
    Task AddPlayersAsync(IEnumerable<PlayerEntity> newPlayer);
    Task UpdatePlayerAsync(int playerId, PlayerEntity updatedPlayer);
    Task RemovePlayerAsync(int playerId);
    Task RemoveAllFplPlayersAsync();
}