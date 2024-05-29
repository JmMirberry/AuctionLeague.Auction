using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.Data.Auction;

public class SoldPlayer : Player
{
    public SoldPlayer(Player player, double salePrice)
    {
        PlayerId = player.PlayerId;
        FirstName = player.FirstName;
        LastName = player.LastName;
        Team = player.Team;
        Position = player.Position;
        Value = player.Value;
        TotalPointsPreviousYear = player.TotalPointsPreviousYear;
        SalePrice = salePrice;
    }

    public SoldPlayer()
    {
    }

    public double SalePrice { get; set; }
}