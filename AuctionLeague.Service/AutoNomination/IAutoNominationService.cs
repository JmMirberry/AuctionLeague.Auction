using AuctionLeague.Data.FplPlayer;
using AuctionLeague.Data.Settings;

namespace AuctionLeague.Service.Interfaces;

public interface IAutoNominationService
{
    Task SetAutoNominations(List<AutonominationSettings> settings);
    Task<Player> GetAutoNomination();
}