using AuctionLeague.Data;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.Service.Auction.Interfaces;
using FluentResults;

namespace AuctionLeague.Service.Auction;

public class AuctionNominationService : IAuctionNominationService
{
    private readonly IPlayerRepository _playerRepository;

    public AuctionNominationService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Result<Player>> NominateByName(string lastNameSearch)
    {
        var playerMatches = (await _playerRepository.GetPlayerAsync(lastNameSearch)).ToList();
        if (!playerMatches.Any())
        {
            return Result.Fail($"No player found with last name matching {lastNameSearch}");
        }
        if (playerMatches.Count > 1)
        {
            return Result.Fail($"Multiple players matched");
        }
        return Result.Ok(playerMatches[0]);
    }

    public async Task<Result<Player>> NominateById(int playerId)
    {
        var player = await _playerRepository.GetPlayerAsync(playerId);
        return player != null ? Result.Ok(player) : Result.Fail($"No player found with id {playerId}");
    }
}