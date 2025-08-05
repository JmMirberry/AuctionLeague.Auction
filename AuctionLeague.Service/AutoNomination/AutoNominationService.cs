using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.Data.Settings;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.Service.Interfaces;
using FluentResults;
using MongoDB.Driver.Linq;

namespace AuctionLeague.Service.AutoNomination;

public class AutoNominationService : IAutoNominationService
{
    private readonly IAuctionPlayerRepository _playerRepository;
    private readonly IAutoNominationRepository _nominationRepository;

    public AutoNominationService(IAuctionPlayerRepository playerRepository, IAutoNominationRepository nominationRepository)
    {
        _playerRepository = playerRepository;
        _nominationRepository = nominationRepository;
    }

    public async Task SetAutoNominations(List<AutonominationSettings> settings)
    {
        var players = await _playerRepository.GetPlayersAsync();

        await SetAutoNomination(settings, players);
    }

    private async Task SetAutoNomination(List<AutonominationSettings> settings, IEnumerable<AuctionPlayer> players)
    {
        var autoNominationRounds = new List<(int round, List<AuctionPlayer>)>();
        var unNominatedPlayers = players.ToList();
        
        foreach (var round in settings)
        {
            var autoNominationPlayers = new List<AuctionPlayer>();

            autoNominationPlayers.AddRange(unNominatedPlayers.Where(x => x.Position == Position.GKP && x.Value >= round.GkpMinValue));
            autoNominationPlayers.AddRange(unNominatedPlayers.Where(x => x.Position == Position.DEF && x.Value >= round.DefMinValue));
            autoNominationPlayers.AddRange(unNominatedPlayers.Where(x => x.Position == Position.MID && x.Value >= round.MidMinValue));
            autoNominationPlayers.AddRange(unNominatedPlayers.Where(x => x.Position == Position.FWD && x.Value >= round.FwdMinValue));

            autoNominationRounds.Add((round.Round, autoNominationPlayers));
            unNominatedPlayers = unNominatedPlayers.Where(a => autoNominationPlayers.All(b => b.PlayerId != a.PlayerId)).ToList();
        }

        await _nominationRepository.RemoveAllAsync();
        await _nominationRepository.AddAutoNominationsAsync(autoNominationRounds);
    }

    public async Task<Result<AuctionPlayer>> GetAutoNomination()
    {
        var rounds = (await _nominationRepository.GetAutoNominationsAsync()).Where(x => x.players.Count > 0).ToList();

        if (rounds == null|| !rounds.Any())
        {
            return Result.Fail("All auto nominations have been nominated");
        }

        var round = rounds.MinBy(x => x.round);
        var players = round.players;
        return await PickAutoNomination(round.round, players);
    }

    public async Task<Result<AuctionPlayer>> GetAutoNominationForRound(int round)
    {
        var rounds = (await _nominationRepository.GetAutoNominationsAsync()).Where(x => x.round == round && x.players.Count > 0)?.ToList();

        if (rounds == null || !rounds.Any())
        {
            return Result.Fail("All auto nominations have been nominated");
        }

        var roundData = rounds[0];
        var players = roundData.players;

        return await PickAutoNomination(roundData.round, players);
    }

    private async Task<Result<AuctionPlayer>> PickAutoNomination(int round, List<AuctionPlayer> players)
    {
        var random = new Random();
        var randomIndex = random.Next(0, players.Count());

        var player = players[randomIndex];
        await _nominationRepository.SetPlayerAsNominated(round, player.PlayerId);
        return Result.Ok(player);
    }
}
