namespace AuctionLeague.MongoDb;

public class AuctionTeamEntity
{
    public string _id { get; set; }
    public string TeamName { get; set; }
    public IEnumerable<string> SlackBidders { get; set; }
    public IEnumerable<SoldPlayerEntity> Players { get; set; } = new List<SoldPlayerEntity>();
    public bool IsComplete => Players.Count() == 11;
}