using AuctionLeague.Data;

namespace AuctionLeague.Fpl;

public interface IFplService
{
    Task<List<Player>> PopulateFplData();
}