using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.Fpl;

public interface IFplService
{
    Task<IEnumerable<Player>> PopulateFplData();
}