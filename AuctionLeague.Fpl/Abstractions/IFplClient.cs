using AuctionLeague.Fpl.Models;

namespace AuctionLeague.Fpl;

public interface IFplClient
{
    Task<FplResponse> GetAllFplData();
}