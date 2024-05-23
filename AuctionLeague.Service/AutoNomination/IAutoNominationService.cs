using AuctionLeague.Data;

namespace AuctionLeague.Service.Interfaces;

public interface IAutoNominationService
{
    Task SetAutoNomination(List<AutonominationSettings> settings);
    Task<Player> GetAutoNomination();
}