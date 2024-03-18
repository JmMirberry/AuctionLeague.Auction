using AuctionLeague.Data;
using Microsoft.Extensions.Options;

namespace AuctionLeague.AuctionService;

public class AuctionService
{
    private readonly Timer _timer;
    private readonly int _timeToOnce;
    private readonly int _timeToTwice;
    private readonly int _timeToSold;
    private int? Bid = null;
    private Player Player = null;
    private string Bidder = null;
    private bool GoingOnce;
    private bool GoingTwice;

    public AuctionService(IOptions<AuctionSettings> settings)
    {
        _timeToOnce = settings.Value.TimeToOnceMs;
        _timeToTwice = settings.Value.TimeToTwiceMs;
        _timeToSold = settings.Value.TimeToSoldMs;
        _timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
    }

    public void NominatePlayer(Player player, string bidder)
    {
        Bid = 1;
        Player = player;
        Bidder = bidder;
    }

    public Player NominatedPlayer()
    {
        return Player;
    }

    public void StartTimer()
    {
        _timer.Change(_timeToOnce, Timeout.Infinite);
    }

    public void EndTimer()
    {
        // Reset the timer interval without stopping it
        _timer.Change(_timeToOnce, Timeout.Infinite);
        ResetData();
    }
    
    public void BidMade( int bid, int playerId, string bidder)
    {
        // Reset the timer interval without stopping it
        _timer.Change(_timeToOnce, Timeout.Infinite);
        Bid = bid;
        Bidder = bidder;
    }

    private void TimerCallback(object state)
    {
        if (!GoingOnce)
        {
            _timer.Change(_timeToTwice, Timeout.Infinite);
            ExecuteGoingOnce();
            GoingOnce = true;
        }
        
        // Save data when there are 3 seconds left
        else if (!GoingTwice)
        {
            _timer.Change(_timeToSold, Timeout.Infinite);
            ExecuteGoingTwice();
            GoingTwice = true;
        }
        
        // Save data when the timer ends
        else
        {
            ExecuteSold();
            ResetData();
        }
    }

    private void ExecuteGoingOnce()
    {
        Console.WriteLine("Going once");
    }

    private void ExecuteGoingTwice()
    {
        Console.WriteLine("Going twice");
    }

    private void ExecuteSold()
    {
        Console.WriteLine("Sold");
    }

    private void ResetData()
    {
        GoingOnce = false;
        GoingTwice = false;
    }
}
