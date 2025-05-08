
namespace AuctionLeague.Service.Auction;

public class PlayerUnavailableException : Exception
{

    public PlayerUnavailableException(string message) : base(message)
    {
    }
}