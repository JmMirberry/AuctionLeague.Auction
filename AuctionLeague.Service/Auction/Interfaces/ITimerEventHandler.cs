
namespace AuctionLeague.Service.Auction.Interfaces
{
    public interface ITimerEventHandler
    {
        Task HandleTimerEnd();
        Task HandleFirstEvent();
        Task HandleSecondEvent();
    }
}
