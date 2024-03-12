using Microsoft.Extensions.Options;

namespace AuctionLeague.AuctionService;

public class AuctionTimer
{
    private readonly Timer _timer;
    private readonly int _timeToOnce;
    private readonly int _timeToTwice;
    private readonly int _timeToSold;
    private int? _bid = null;
    private int? _playerId = null;
    private string _bidder = null;
    private bool _goingOnce;
    private bool _goingTwice;

    public AuctionTimer(IOptions<AuctionSettings> settings)
    {
        _timeToOnce = settings.Value.TimeToOnceMs;
        _timeToTwice = settings.Value.TimeToTwiceMs;
        _timeToSold = settings.Value.TimeToSoldMs;
        _timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
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
        _bid = bid;
        _playerId = playerId;
        _bidder = bidder;
    }

    private void TimerCallback(object state)
    {
        if (!_goingOnce)
        {
            _timer.Change(_timeToTwice, Timeout.Infinite);
            Console.WriteLine("Data saved at 5 seconds");
            _goingOnce = true;
        }
        
        // Save data when there are 3 seconds left
        else if (!_goingTwice)
        {
            _timer.Change(_timeToSold, Timeout.Infinite);
            Console.WriteLine("Data saved at 3 seconds");
            _goingTwice = true;
        }
        
        // Save data when the timer ends
        else
        {
            Console.WriteLine("Data saved at timer completion");
            ResetData();
        }
    }
    
    private void ResetData()
    {
        _goingOnce = false;
        _goingTwice = false;
    }
}
