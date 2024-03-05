using AuctionLeague.Data;
using AuctionLeague.Fpl.Models;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.MongoDb.Mappers;

namespace AuctionLeague.Fpl;

public class FplService : IFplService
{
    private readonly IFplClient _client;
    private readonly IPlayerRepository _repository;

    public FplService(IFplClient client, IPlayerRepository repository)
    {
        _client = client;
        _repository = repository;
    }

    public async Task<List<Player>> PopulateFplData()
    {
        var data = await _client.GetAllFplData();
        var mappedPlayers = MapPlayers(data);
        await _repository.RemoveAllFplPlayersAsync();
        await _repository.AddPlayersAsync(mappedPlayers.Select(p => p.ToEntity()));
        return mappedPlayers;
    }

    private static List<Player> MapPlayers(FplResponse response)
    {
        var mappedPlayers = response.elements.Select(p =>
            new Player
            {
                PlayerId = p.id,
                FirstName = p.first_name,
                LastName = p.second_name,
                Team = response.teams.First(t => t.id == p.team).short_name,
                Position = response.element_types.First(e => e.id == p.element_type).singular_name_short,
                Value = p.now_cost / 10.0,
                TotalPointsPreviousYear = p.total_points,
                IsInFpl = true
            }).ToList();
        return mappedPlayers;
    }
}