using AuctionLeague.Data;
using AuctionLeague.MongoDb.Abstractions;
using MongoDB.Driver.Linq;

namespace AuctionLeague.Service;

public class AutoNominationService : IAutoNominationService
{
    private readonly IAutoNominationRepository _repository;
    private readonly IPlayerRepository _playerRepository;

    public AutoNominationService(IAutoNominationRepository repository, IPlayerRepository playerRepository)
    {
        _repository = repository;
        _playerRepository = playerRepository;
    }

    public async Task SetAutoNomination(List<AutonominationSettings> settings)
    {
        var players = await _playerRepository.GetPlayersAsync();

        await SetAutoNomination(settings, players);
    }

    private async Task SetAutoNomination(List<AutonominationSettings> settings, IEnumerable<Player> players)
    {
        var playersByPosition = players.GroupBy(x => x.Position);

        var autoNominationPlayers = new List<Player>();

        foreach (var grouping in playersByPosition)
        {
            var minValue = settings.Find(x => x.Position == grouping.Key).MinValue;
            autoNominationPlayers.AddRange(grouping.Where(g => g.Value >= minValue));
        }

        await _repository.RemoveAutonominationDataAsync();
        await _repository.SaveAutoNominationDataAsync(autoNominationPlayers);
    }

    public async Task<Player> GetAutoNomination()
    {
        var players = (await _repository.GetAutoNominationsAsync()).ToList();
        var random = new Random();
        var randomIndex = random.Next(0, players.Count());

        return players[randomIndex];
    }
}
