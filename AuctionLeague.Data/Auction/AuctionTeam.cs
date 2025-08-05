namespace AuctionLeague.Data.Auction;

public class AuctionTeam
{
    public string TeamName { get; set; }
    public List<string> SlackBidders { get; set; } = new List<string>();
    public List<SoldPlayer> Players { get; set; } = new List<SoldPlayer>();
    public bool IsComplete => Players.Count() == 11;
}
