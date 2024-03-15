using AuctionLeague.Data;

namespace AuctionLeague.MongoDb;

public class SoldPlayerEntity : PlayerEntity
{
    public SoldPlayerEntity(PlayerEntity player, double salePrice)
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

    public SoldPlayerEntity() { }
    public double SalePrice { get; set; }
}