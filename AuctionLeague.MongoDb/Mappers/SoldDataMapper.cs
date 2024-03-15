using AuctionLeague.Data;
using AuctionLeague.MongoDb;

public static class SoldDataMapper
{
    public static SoldDataEntity ToEntity(this SoldData player)
    {
        return new SoldDataEntity
        {
            _id = player.PlayerId.ToString(),
            PlayerId = player.PlayerId,
            FirstName = player.FirstName,
            LastName = player.LastName,
            SalePrice = player.SalePrice,
            SoldTo = player.SoldTo
        };
    }

    public static SoldData ToSoldData(this SoldDataEntity player)
    {
        return new SoldData
        {
            PlayerId = player.PlayerId,
            FirstName = player.FirstName,
            LastName = player.LastName,
            SalePrice = player.SalePrice,
            SoldTo = player.SoldTo
        };
    }
}
