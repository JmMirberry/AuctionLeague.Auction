using AuctionLeague.Data.Auction;

namespace AuctionLeague.Service.Auction;

public class PlayerNotFoundException : Exception
{

    public PlayerNotFoundException(string message) : base(message)
    {
    }
}