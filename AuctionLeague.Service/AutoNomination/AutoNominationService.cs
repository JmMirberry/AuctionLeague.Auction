using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.Data.Settings;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.Service.Interfaces;
using MongoDB.Driver.Linq;

namespace AuctionLeague.Service.AutoNomination;

public class AutoNominationService : IAutoNominationService
{
    private readonly IAuctionPlayerRepository _playerRepository;

    public AutoNominationService( IAuctionPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task SetAutoNominations(List<AutonominationSettings> settings)
    {
        var players = await _playerRepository.GetPlayersAsync();

        await SetAutoNomination(settings, players);
    }

    private async Task SetAutoNomination(List<AutonominationSettings> settings, IEnumerable<AuctionPlayer> players)
    {
        var playersByPosition = players.GroupBy(x => x.Position);

        var autoNominationPlayers = new List<AuctionPlayer>();

        foreach (var grouping in playersByPosition)
        {
            var minValue = settings.Find(x => x.Position == grouping.Key).MinValue;
            autoNominationPlayers.AddRange(grouping.Where(g => g.Value >= minValue));
        }

        await _playerRepository.ResetAutoNomination();
        await _playerRepository.SetAutoNominations(autoNominationPlayers);
    }

    public async Task<Player> GetAutoNomination()
    {
        var players = (await _playerRepository.GetAutoNominations()).ToList();
        var random = new Random();
        var randomIndex = random.Next(0, players.Count());

        var player = players[randomIndex];

        return player;
    }
}
