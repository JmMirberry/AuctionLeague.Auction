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

    public async Task<AuctionPlayer> NominateByName(string lastNameSearch)
    {
        var playerMatches = (await _playerRepository.GetPlayerAsync(lastNameSearch)).ToList();
        if (!playerMatches.Any())
        {
            throw new PlayerNotFoundException($"No player found with last name matching {lastNameSearch}");
        }
        if (playerMatches.Count > 1)
        {
            throw new PlayerNotFoundException("Multiple players matched");
        }

        var player = playerMatches[0];

        if (player.IsSold)
        {
            throw new PlayerUnavailableException("Player is has already been sold");
        }
        return playerMatches[0];
    }

    public async Task<AuctionPlayer> NominateById(int playerId)
    {
        var player = await _playerRepository.GetPlayerAsync(playerId);

        if (player == null )
        {
            throw new PlayerNotFoundException($"No player found with id {playerId}");
        }

        if (player.IsSold)
        {
            throw new PlayerUnavailableException($"Player is has already been sold");
        }

        return player;
    }
}