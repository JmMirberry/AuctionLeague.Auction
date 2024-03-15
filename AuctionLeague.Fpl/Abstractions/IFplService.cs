using AuctionLeague.Data;

namespace AuctionLeague.Fpl;

public interface IFplService
{
    Task<IEnumerable<Player>> PopulateFplData();
}