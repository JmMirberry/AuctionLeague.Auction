namespace AuctionLeague.Data;

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
        IsInFpl = player.IsInFpl;
        SalePrice = salePrice;
    }


    public double SalePrice { get; set; }
}