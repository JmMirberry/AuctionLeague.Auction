namespace AuctionLeague.AuctionService;

public class AuctionSettings
{
    public int TimeToOnceMs { get; set; } = 5000;
    public int TimeToTwiceMs { get; set; } = 3000;
    public int TimeToSoldMs { get; set; } = 2000;
}