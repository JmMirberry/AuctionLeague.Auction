namespace AuctionLeague.Fpl.Models;

public class FplResponse
{
    public FplTeam[] teams { get; set; }
    public FplPlayer[] elements { get; set; }
    public FplPosition[] element_types { get; set; }
}
