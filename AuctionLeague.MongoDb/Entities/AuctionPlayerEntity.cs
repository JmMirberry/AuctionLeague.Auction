using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.MongoDb;

public class AuctionPlayerEntity : PlayerEntity
{
    public bool IsAutoNomination { get; set; }
    public bool IsFplPlayer { get; set; }
    public bool IsSold { get; set; }
}