using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.MongoDb;

public class AutoNominatedPlayerEntity : PlayerEntity
{
    public bool Nominated { get; set; }
}