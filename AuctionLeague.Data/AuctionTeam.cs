namespace AuctionLeague.Data;

public class AuctionTeam
{
    public string TeamName { get; set; }
    public List<string> SlackBidders { get; set; }
    public List<SoldPlayer> Players { get; set; }
    public bool IsComplete => Players.Count == 11;
}