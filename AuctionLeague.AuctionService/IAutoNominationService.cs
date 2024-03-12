using AuctionLeague.Data;

namespace AuctionLeague.AuctionService;

public interface IAutoNominationService
{
    Task SetAutoNomination(List<AutonominationSettings> settings);
    Task<Player> GetAutoNomination();
}