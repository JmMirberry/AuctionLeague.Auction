using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.MongoDb;

public class AutoNominationEntity
{
    public string _id {  get; set; }
    public int Round { get; set; }
    public List<AutoNominatedPlayerEntity> Players { get; set; }
}
