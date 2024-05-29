namespace AuctionLeague.Service.Auction.Interfaces;

public interface IAuctionTimer
{
    bool TimerRunning();
    void StartTimer();
    void RestartTimer();
    void ResetTimer();
}