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

        foreach (var round in settings)
        {
            var autoNominationPlayers = new List<AuctionPlayer>();

            autoNominationPlayers.AddRange(players.Where(x => x.Position == Position.GKP && x.Value >= round.GkpMinValue));
            autoNominationPlayers.AddRange(players.Where(x => x.Position == Position.DEF && x.Value >= round.DefMinValue));
            autoNominationPlayers.AddRange(players.Where(x => x.Position == Position.MID && x.Value >= round.MidMinValue));
            autoNominationPlayers.AddRange(players.Where(x => x.Position == Position.FWD && x.Value >= round.FwdMinValue));

            autoNominationRounds.Add((round.Round, autoNominationPlayers));
        }

        await _nominationRepository.RemoveAllAsync();
        await _nominationRepository.AddAutoNominationsAsync(autoNominationRounds);
    }

    public async Task<Result<AuctionPlayer>> GetAutoNomination()
    {
        var rounds = (await _nominationRepository.GetAutoNominationsAsync()).Where(x => x.player.Count > 0);

        if (rounds == null)
        {
            return Result.Fail("All autonominations have been nominated");
        }

        var round = rounds.OrderBy(x => x.round).First();
        var players = round.player;
        return await PickAutoNomination(round.round, players);
    }

    public async Task<Result<AuctionPlayer>> GetAutoNominationForRound(int round)
    {
        var rounds = (await _nominationRepository.GetAutoNominationsAsync()).Where(x => x.round == round && x.player.Count > 0)?.ToList();

        if (rounds == null || rounds.Count() == 0)
        {
            return Result.Fail("All autonominations have been nominated");
        }

        var roundData = rounds[0];
        var players = roundData.player;

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
