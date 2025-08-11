using AuctionLeague.Data;

namespace AuctionLeague.MongoDb;

public class SoldPlayerEntity : PlayerEntity
{
    public SoldPlayerEntity(PlayerEntity player, double salePrice, DateTime saleTime)
    {
        PlayerId = player.PlayerId;
        FirstName = player.FirstName;
        LastName = player.LastName;
        Team = player.Team;
        Position = player.Position;
        Value = player.Value;
        TotalPointsPreviousYear = player.TotalPointsPreviousYear;
        SalePrice = salePrice;
        SaleTime = saleTime;
    }

    public SoldPlayerEntity() { }
    public double SalePrice { get; set; }
    public DateTime SaleTime { get; set; }
}
