using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.Service.Auction.Interfaces;
using FluentResults;

namespace AuctionLeague.Service.Auction;

public class AuctionNominationService : IAuctionNominationService
{
    private readonly IAuctionPlayerRepository _playerRepository;

    public AuctionNominationService(IAuctionPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Result<AuctionPlayer>> NominateByName(string lastNameSearch)
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

        var player = playerMatches[0];

        if (player.IsSold)
        {
            return Result.Fail($"Player is has already been sold");
        }
        return Result.Ok(playerMatches[0]);
    }

    public async Task<Result<AuctionPlayer>> NominateById(int playerId)
    {
        var player = await _playerRepository.GetPlayerAsync(playerId);

        if (player == null )
        {
            return Result.Fail($"No player found with id {playerId}");
        }

        if (player.IsSold)
        {
            return Result.Fail($"Player is has already been sold");
        }

        return Result.Ok(player);
    }
}