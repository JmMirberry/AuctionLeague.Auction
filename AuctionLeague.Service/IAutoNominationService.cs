using AuctionLeague.Data;

namespace AuctionLeague.Service;

public interface IAutoNominationService
{
    Task SetAutoNomination(List<AutonominationSettings> settings);
    Task<Player> GetAutoNomination();
}